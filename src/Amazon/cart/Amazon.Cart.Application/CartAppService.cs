using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Mappers;
using Amazon.Cart.Application.Payments;
using Amazon.Cart.Application.Payments.Validators;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Domain.Specifications;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Extensions;
using Amazon.SharedKernel.IntegrationEvents.ShoppingCart;
using Amazon.SharedKernel.Orders.Events;
using FluentValidation;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Cart.Application
{
    public class CartAppService(
        CartService _cartService,
        IRepository<ShoppingCart, Guid> _cartsRepo,
        IRepository<Product, Guid> _products,
        IRepository<Transaction, Guid> _transactions,
        IUnitOfWork _unitOfWork,
        IOtpService _otpService,
        IAuthenticationService _authenticationService,
        PaymentsAppService _paymentsAppService,
        PaymentsService _paymentsService,
        ShoppingCartSpecification _specification,
        IRepository<ShoppingCart, Guid> _carts,
        EventStoreService _eventStoreService
        )
    {
        private readonly CurrentUser _currentUser = _authenticationService.CurrentUser;
        private Guid _currentUserId => _currentUser.Id;

        public async Task<RestResponse<List<CartItemDto>>> GetByIdAsync(Guid cartId)
        {
            var cartResult = await _cartService.GetByIdAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(null as List<CartItemDto>);

            var products = await _products.GetAllAsync(x => cartResult.Value.AggregatToProducts.Select(x => x.Key).Contains(x.Id));
            return RestResponse<List<CartItemDto>>.Success(cartResult.Value.ToItemsDto(products));
        }

        public async Task<RestResponse<CartCreateResultDto>> CreateCartAsync(CartCreateDto createDto)
        {
            var cartCreateResult = await _cartService.CreateCartAsync(_currentUser.IsAuthenticated ? _currentUser.Id : null);
            if (!cartCreateResult.IsSuccess)
                return cartCreateResult.MapTo((CartCreateResultDto)null);

            var result = await _cartService.TryAddItemToCartAsync(cartCreateResult, createDto.CartItem.ProductId);

            var response = RestResponse<CartCreateResultDto>.Success(new(cartCreateResult.Value.Id, 0));

            if (!result.IsSuccess)
                response.WithMessage(result.Error.ToString()!);

            await _unitOfWork.CommitAsync();
            return response;
        }

        public async Task<RestResponse<int>> AddItemToCartAsync(Guid cartId, CartItemCreateDto cartItemDto)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId);
            if (cart is null)
                return RestResponse<int>.NotFound($"Cart with id {cartId} was not found");

            var result = await _cartService.TryAddItemToCartAsync(cart, cartItemDto.ProductId);
            if (!result.IsSuccess)
                return result.MapTo((int)0);

            await _unitOfWork.CommitAsync();
            return RestResponse<int>.Success(0);
        }

        public async Task<RestResponse<bool>> RemoveItemFromCartAsync(Guid cartId, Guid productId)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId);
            if (cart is null)
                return RestResponse<bool>.NotFound($"Cart with id {cartId} was not found");

            cart.RemoveItem(productId);
            await _unitOfWork.CommitAsync();

            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> RemoveAllProductItemsAsync(Guid cartId, Guid productId)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId);
            if (cart is null)
                return RestResponse<bool>.NotFound($"Cart with id {cartId} was not found");

            cart.RemoveProductItems(productId);
            await _unitOfWork.CommitAsync();

            return RestResponse<bool>.Success(true);
        }

        public async Task PurgeExpiredCartsAsync()
        {
            var expiredCarts = await _cartsRepo.GetAllAsync(x => x.Expiration.ExpiresAt <= DateTime.UtcNow);
            foreach (var expiredCart in expiredCarts)
            {
                expiredCart.RaiseEvent(new CartExpiredEvent([.. expiredCart.Items.Select(x => x.ProductId).Distinct().ToList()]));
                _cartsRepo.Remove(expiredCart);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task<RestResponse<int>> SetupForCheckoutAsync(Guid cartId, UpdateCartDto updateCartDto)
        {
            var setupResult = await _cartService.SetupForCheckoutAsync(cartId, _currentUserId, updateCartDto.DeliverToAddressId, updateCartDto.PaymentMethodId);
            if (!setupResult.IsSuccess)
                return setupResult.MapTo(-1);

            await _unitOfWork.CommitAsync();
            return setupResult;
        }

        public async Task<RestResponse<Guid>> CheckoutCartUsingOtpAsync(Guid cartId, string otp)
        {
            var cartResult = await _cartService.GetByIdForUserAsync(cartId, _currentUserId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(Guid.Empty);

            var isCartStateSatisfied = await _specification.SatisfiesAsync(cartResult);
            if (!isCartStateSatisfied.IsSuccess)
                return isCartStateSatisfied.MapTo(Guid.Empty);

            var isOtpValid = await _otpService.ValidateAsync(_currentUserId, otp);
            if (!isOtpValid)
                return RestResponse<Guid>.BadRequest($"Invalid otp {otp}");

            var orderCreateResult = await _cartService.CreateOrderAsync(cartResult, _currentUserId);
            if (!orderCreateResult.IsSuccess)
                return orderCreateResult.MapTo(Guid.Empty);

            await _unitOfWork.CommitAsync();
            return RestResponse<Guid>.Success(orderCreateResult.Value);
        }

        public async Task<RestResponse<Guid>> CheckoutCartUsingVisaAsync(Guid cartId, CheckoutUsingVisaRequest request)
        {
            var cartResult = await _cartService.GetByIdAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(Guid.Empty);

            var isCartStateSatisfied = await _specification.SatisfiesAsync(cartResult);
            if (!isCartStateSatisfied.IsSuccess)
                return isCartStateSatisfied.MapTo(Guid.Empty);

            var canPaymentCardSatisfyOrder = await _paymentsService.ChargePaymentCardForAmountAsync(_currentUserId, request.PaymentCardId, request.Cvv, cartResult.Value.TotalAmount);
            if (!canPaymentCardSatisfyOrder.IsSuccess)
                return RestResponse<Guid>.BadRequest(canPaymentCardSatisfyOrder.Error.ToString());

            var orderCreateResult = await _cartService.CreateOrderAsync(cartResult, _currentUserId);
            if (!orderCreateResult.IsSuccess)
                return orderCreateResult.MapTo(Guid.Empty);

            await _unitOfWork.CommitAsync();
            return RestResponse<Guid>.Success(orderCreateResult.Value);
        }

        public async Task<RestResponse<ChallengePaymentResponse>> ChallengePaymentAndCreateOrderAsync(Guid cartId, ChallengePaymentRequest request)
        {
            var cartResult = await _cartService.GetForCheckoutAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(null as ChallengePaymentResponse);

            Guid orderId = Guid.Empty;

            if (!cartResult.Value.OrderId.HasValue)
            {
                var orderCreateResult = await _cartService.CreateOrderAsync(cartResult, _currentUserId);
                if (!orderCreateResult.IsSuccess)
                    return orderCreateResult.MapTo(null as ChallengePaymentResponse);

                orderId = orderCreateResult.Value;

                cartResult.Value.SetOrder(orderId);
            }

            var checkoutResponse = await _paymentsAppService.ChallengePaymentAsync(orderId, request.PaymentMethodId, _currentUserId, request.DeliverToAddressId);
            cartResult.Value.SetPaymentMethod(checkoutResponse.Value.PaymentMehod);

            await _unitOfWork.CommitAsync();
            return RestResponse<ChallengePaymentResponse>.Success(checkoutResponse);
        }


        public async Task<RestResponse<Guid>> ConfirmPaymentAsync(Guid cartId, ConfirmPaymentRequest request)
        {
            var cartResult = await _cartService.GetForCheckoutAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(Guid.Empty);

            if (!cartResult.Value.OrderId.HasValue || !cartResult.Value.PaymentMethod.HasValue)
                return RestResponse<Guid>.BadRequest("Cart has not been checed out  yet!");

            switch (cartResult.Value.PaymentMethod.Value)
            {
                case PaymentMehodType.Cash:
                    if (string.IsNullOrWhiteSpace(request.Otp))
                        return RestResponse<Guid>.BadRequest("Please provide a valid otp");

                    var isOtpValid = await _otpService.ValidateAsync(_currentUserId, request.Otp);
                    if (!isOtpValid)
                        return RestResponse<Guid>.BadRequest($"Invalid otp {request.Otp}");

                    _eventStoreService.Append(new OrderPaymentConfirmedEvent(cartResult.Value.OrderId.Value));
                    break;

                case PaymentMehodType.Visa:
                    var validationResult = new CheckoutUsingVisaRequestValidator().Validate(request.VisaDetails);
                    if (!validationResult.IsValid)
                        return RestResponse<Guid>.BadRequest(validationResult.Errors.FirstOrDefault().ErrorMessage);

                    var chargeCardResult = await _paymentsService.ChargePaymentCardForAmountAsync(_currentUserId, request.VisaDetails.PaymentCardId, request.VisaDetails.Cvv, cartResult.Value.TotalAmount);
                    if (!chargeCardResult.IsSuccess)
                        return RestResponse<Guid>.BadRequest(chargeCardResult.Error.ToString());

                    var transaction = new Transaction(cartResult.Value.TotalAmount, cartResult.Value.OrderId.Value, _currentUserId, request.VisaDetails.PaymentCardId, chargeCardResult.Value);
                    _transactions.Add(transaction);

                    _eventStoreService.Append(new OrderPaymentConfirmedEvent(cartResult.Value.OrderId.Value, transaction.Id));
                    break;

                default:
                    return RestResponse<Guid>.Failure(new InvalidOperationException("Payment method is not supported"));
            }

            _carts.Remove(cartResult.Value);
            await _unitOfWork.CommitAsync();
            return RestResponse<Guid>.Success(cartResult.Value.OrderId.Value);
        }
    }
}
