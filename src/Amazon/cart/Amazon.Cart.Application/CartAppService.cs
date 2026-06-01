using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Mappers;
using Amazon.Cart.Application.Payments;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.Payments;
using Amazon.Cart.Domain.Services;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common.Services;
using Amazon.SharedKernel.Extensions;
using Amazon.SharedKernel.IntegrationEvents.ShoppingCart;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Application
{
    public class CartAppService(
        CartService _cartService,
        IRepository<ShoppingCart, Guid> _cartsRepo,
        IUnitOfWork _unitOfWork,
        IOtpService _otpService,
        IAuthenticationService _authenticationService,
        PaymentsAppService _paymentsAppService,
        PaymentsService _paymentsService
        )
    {
        private readonly CurrentUser _currentUser = _authenticationService.CurrentUser;
        private Guid _currentUserId => _currentUser.Id;

        public async Task<RestResponse<List<CartItemDto>>> GetByIdAsync(Guid cartId)
        {
            var cartResult = await _cartService.GetByIdAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(null as List<CartItemDto>);

            return RestResponse<List<CartItemDto>>.Success(cartResult.Value.ToItemsDto());
        }

        public async Task<RestResponse<CartCreateResultDto>> CreateCartAsync(CartCreateDto createDto)
        {
            var cartCreateResult = await _cartService.CreateCartAsync(_currentUser.IsAuthenticated ? _currentUser.Id : null);
            if (!cartCreateResult.IsSuccess)
                return cartCreateResult.MapTo((CartCreateResultDto)null);

            var result = await AddItemToCartAsync(cartCreateResult, createDto.CartItem);
            await _unitOfWork.CommitAsync();

            var response = RestResponse<CartCreateResultDto>.Success(new(cartCreateResult.Value.Id, result.Value?.Id ?? 0));

            if (!result.IsSuccess)
                response.WithMessage(result.Error.ToString()!);

            return response;
        }

        public async Task<RestResponse<int>> AddItemToCartAsync(Guid cartId, CartItemCreateDto cartItemDto)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId);
            if (cart is null)
                return RestResponse<int>.NotFound($"Cart with id {cartId} was not found");

            var result = await AddItemToCartAsync(cart, cartItemDto);
            if (!result.IsSuccess)
                return result.MapTo((int)0);

            await _unitOfWork.CommitAsync();
            return RestResponse<int>.Success(result.Value.Id);
        }

        public async Task<RestResponse<bool>> RemoveItemFromCartAsync(Guid cartId, int cartItemId)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId);
            if (cart is null)
                return RestResponse<bool>.NotFound($"Cart with id {cartId} was not found");

            cart.RemoveItem(cartItemId);
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
            var cartResult = await _cartService.GetByIdAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(-1);

            var setupResult = await _cartService.SetupForCheckoutAsync(cartResult, _currentUserId, updateCartDto.DeliverToAddressId, updateCartDto.PaymentMethodId);
            if (!setupResult.IsSuccess)
                return setupResult.MapTo(-1);

            await _unitOfWork.CommitAsync();
            return setupResult;
        }

        public async Task<RestResponse<Guid>> CheckoutCartUsingOtpAsync(Guid cartId, string otp)
        {
            var isOtpValid = await _otpService.ValidateAsync(_currentUserId, otp);
            if (!isOtpValid)
                return RestResponse<Guid>.BadRequest($"Invalid otp {otp}");

            var orderCreateResult = await _cartService.TryCheckoutAsync(cartId, _currentUserId, new { PaymentMethod = "Cash" });
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

            var canPaymentCardSatisfyOrder = await _paymentsService.TryWithdrawFromPaymentCardAsync(_currentUserId, request.PaymentCardId, request.Cvv, cartResult.Value.TotalAmount);
            if (!canPaymentCardSatisfyOrder.IsSuccess)
                return RestResponse<Guid>.BadRequest(canPaymentCardSatisfyOrder.Error.ToString());

            var orderCreateResult = await _cartService.TryCheckoutAsync(cartId, _currentUserId, new { PaymentMethod = "Visa", CardNumber = canPaymentCardSatisfyOrder.Value });
            if (!orderCreateResult.IsSuccess)
                return orderCreateResult.MapTo(Guid.Empty);

            await _unitOfWork.CommitAsync();
            return RestResponse<Guid>.Success(orderCreateResult.Value);
        }

        private async Task<RestResponse<CartItem>> AddItemToCartAsync(ShoppingCart cart, CartItemCreateDto cartItem)
        {
            return await _cartService.TryAddItemToCartAsync(cart, cartItem.ProductId);
        }

    }
}
