using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Mappers;
using Amazon.Cart.Application.Payments;
using Amazon.Cart.Application.Payments.Challenge;
using Amazon.Cart.Application.Services;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.Services;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Amazon.SharedKernel.Orders.Events;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.Cart.Application
{
    public class CartAppService(
        CartService _cartService,
        IRepository<ShoppingCart, Guid> _carts,
        IProductsRepo _products,
        IUnitOfWork _unitOfWork,
        IAuthenticationService _authenticationService,
        EventStoreService _eventStoreService,
        PaymentMethodChallengeStartegy _paymentMethodChallengeStartegy,
        IPaymentMethodChallengeHandlerFactory _factory
        )
    {
        private readonly CurrentUser _currentUser = _authenticationService.CurrentUser;
        private Guid _currentUserId => _currentUser.Id;

        public async Task<RestResponse<CartCreateResultDto>> CreateCartAsync(CartCreateDto createDto)
        {
            var shoppingCart = await _cartService.EnsureCartExitsAsync(_currentUser.IsAuthenticated ? _currentUser.Id : null);

            var cartAddResult = await TryAddItemToCartAsync(shoppingCart, createDto.CartItem.ProductId);
            if (!cartAddResult.IsSuccess)
                return cartAddResult.MapTo(null as CartCreateResultDto);

            _carts.Add(shoppingCart);

            await _unitOfWork.CommitAsync();
            return RestResponse<CartCreateResultDto>.Success(new CartCreateResultDto(shoppingCart.Id, cartAddResult.Value));
        }

        public async Task<RestResponse<List<CartItemDto>>> GetByIdAsync(Guid cartId)
        {
            var cartResult = await _cartService.GetByIdAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(null as List<CartItemDto>);

            var products = await _products.GetCartProductsAsync(cartResult);

            return RestResponse<List<CartItemDto>>.Success(cartResult.Value.ToItemsDto(products));
        }

        public async Task<RestResponse<CartItemDto>> AddItemToCartAsync(Guid cartId, CartItemCreateDto cartItemDto)
        {
            var cart = await _cartService.GetByIdAsync(cartId);
            if (!cart.IsSuccess)
                return cart.MapTo(null as CartItemDto);

            var result = await TryAddItemToCartAsync(cart, cartItemDto.ProductId);
            if (result.IsSuccess)
                await _unitOfWork.CommitAsync();

            return result;
        }

        private async Task<RestResponse<CartItemDto>> TryAddItemToCartAsync(ShoppingCart shoppingCart, Guid productId)
        {
            var addResult = await _cartService.TryAddItemToCartAsync(shoppingCart, productId);

            if (!addResult.IsSuccess)
                return addResult.MapTo(null as CartItemDto);

            return RestResponse<CartItemDto>.Success(new CartItemDto(addResult.Value.ProductId, addResult.Value.Info.Name, addResult.Value.Info.ImageUrl, addResult.Value.Quantity, addResult.Value.Info.UnitPrice, true));
        }

        public async Task<RestResponse<bool>> RemoveItemFromCartAsync(Guid cartId, Guid productId)
        {
            var cart = await _cartService.GetByIdAsync(cartId);
            if (!cart.IsSuccess)
                return cart.MapTo(false);

            cart.Value.PopProductItem(productId);

            return await CommitAsync();
        }

        public async Task<RestResponse<bool>> RemoveAllProductItemsAsync(Guid cartId, Guid productId)
        {
            var cart = await _cartService.GetByIdAsync(cartId);
            if (!cart.IsSuccess)
                return cart.MapTo(false);

            cart.Value.RemoveProductItems(productId);

            return await CommitAsync();
        }

        public async Task<RestResponse<ChallengePaymentResponse>> CreateOrderAndChallengePaymentAsync(Guid cartId, ChallengePaymentRequest request)
        {
            var shoppingCart = await _cartService.EnsureCartHasOrderAsync(cartId);
            if (!shoppingCart.IsSuccess)
                return shoppingCart.MapTo(null as ChallengePaymentResponse);

            var checkoutResponse = await _paymentMethodChallengeStartegy.ChallengeCustomerAsync(shoppingCart, request.PaymentMethodId, _currentUserId, request.DeliverToAddressId);
            if (!checkoutResponse.IsSuccess)
                return checkoutResponse;

            return (await CommitAsync()).MapTo(checkoutResponse);
        }

        public async Task<RestResponse<Guid>> ConfirmPaymentAsync(Guid cartId, ConfirmPaymentRequest request)
        {
            var cartResult = await _cartService.GetForCheckoutConfimrationAsync(cartId);
            if (!cartResult.IsSuccess)
                return cartResult.MapTo(Guid.Empty);

            var paymentConfimration = await HandlePaymentConfirmationAsync(cartResult, request);
            if (!paymentConfimration.IsSuccess)
                return paymentConfimration.MapTo(Guid.Empty);

            _eventStoreService.Append(new OrderPaymentConfirmedEvent(cartResult.Value.OrderId.Value, paymentConfimration));
            _carts.Remove(cartResult.Value);

            return (await CommitAsync()).MapTo(cartResult.Value.OrderId.Value);
        }

        private async Task<RestResponse<CheckoutPaymentInfo>> HandlePaymentConfirmationAsync(ShoppingCart shoppingCart, ConfirmPaymentRequest request)
        {
            var confimrationHandler = _factory.CreateForConfirmation(shoppingCart.PaymentMethod.Value);

            return await confimrationHandler.ConfirmAsync(request, _currentUserId, shoppingCart.TotalAmount);
        }

        private async Task<RestResponse<bool>> CommitAsync()
        {
            await _unitOfWork.CommitAsync();
            return RestResponse<bool>.Success(true);
        }
    }
}
