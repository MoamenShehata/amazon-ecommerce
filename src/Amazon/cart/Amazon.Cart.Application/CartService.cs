using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Mappers;
using Amazon.Cart.Domain;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Application
{
    public class CartService(
        ShoppingCartService _cartService,
        IRepository<ShoppingCart, Guid> _cartsRepo,
        IUnitOfWork _unitOfWork
        )
    {
        public async Task<RestResponse<List<CartItemDto>>> GetByIdAsync(Guid cartId)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId && x.Expiration.ExpiresAt > DateTime.UtcNow);
            if (cart is null)
                return RestResponse<List<CartItemDto>>.NotFound($"Cart with id {cartId} was not found");


            return RestResponse<List<CartItemDto>>.Success(cart.ToItemsDto());
        }

        public async Task<RestResponse<CartItemDto>> CreateCartAsync(CartCreateDto createDto)
        {
            var cartCreateResult = await _cartService.CreateCartAsync(createDto.CustomerId);
            if (!cartCreateResult.IsSuccess)
                return cartCreateResult.MapTo((CartItemDto)null);

            var cartItemDto = AddItemToCart(cartCreateResult, createDto.CartItem);

            await _unitOfWork.CommitAsync();

            return RestResponse<CartItemDto>.Success(cartItemDto);
        }

        public async Task<RestResponse<CartItemDto>> AddItemToCartAsync(Guid cartId, CartItemCreateDto cartItem)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId && x.Expiration.ExpiresAt > DateTime.UtcNow);
            if (cart is null)
                return RestResponse<CartItemDto>.NotFound($"Cart with id {cartId} was not found");

            var cartItemDto = AddItemToCart(cart, cartItem);

            await _unitOfWork.CommitAsync();

            return RestResponse<CartItemDto>.Success(cartItemDto);
        }

        private CartItemDto AddItemToCart(ShoppingCart cart, CartItemCreateDto cartItem)
        {
            var itemId = cart.AddItem(cartItem.ProductId, cartItem.Quantity, cartItem.ProductName, cartItem.ProductImageUrl);

            return new CartItemDto(cart.Id, itemId, cartItem.ProductId, cartItem.Quantity, cartItem.ProductName, cartItem.ProductImageUrl);
        }
    }
}
