using System.Threading.Tasks;
using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Application.Mappers;
using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Entities;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using MassTransit;
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
        public async Task<RestResponse<List<CartProductDto>>> GetByIdAsync(Guid cartId)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId && x.Expiration.ExpiresAt > DateTime.UtcNow);
            if (cart is null)
                return RestResponse<List<CartProductDto>>.NotFound($"Cart with id {cartId} was not found");


            return RestResponse<List<CartProductDto>>.Success(cart.ToItemsDto());
        }

        public async Task<RestResponse<CartCreateResultDto>> CreateCartAsync(CartCreateDto createDto)
        {
            var cartCreateResult = await _cartService.CreateCartAsync(createDto.CustomerId);
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
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId && x.Expiration.ExpiresAt > DateTime.UtcNow);
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
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId && x.Expiration.ExpiresAt > DateTime.UtcNow);
            if (cart is null)
                return RestResponse<bool>.NotFound($"Cart with id {cartId} was not found");

            cart.RemoveItem(cartItemId);
            await _unitOfWork.CommitAsync();

            return RestResponse<bool>.Success(true);
        }

        public async Task<RestResponse<bool>> RemoveAllProductItemsAsync(Guid cartId, Guid productId)
        {
            var cart = await _cartsRepo.GetInstanceAsync(x => x.Id == cartId && x.Expiration.ExpiresAt > DateTime.UtcNow);
            if (cart is null)
                return RestResponse<bool>.NotFound($"Cart with id {cartId} was not found");

            cart.RemoveProductItems(productId);
            await _unitOfWork.CommitAsync();

            return RestResponse<bool>.Success(true);
        }

        private async Task<RestResponse<CartItem>> AddItemToCartAsync(ShoppingCart cart, CartItemCreateDto cartItem)
        {
            return await _cartService.TryAddItemToCartAsync(cart, cartItem.ProductId, cartItem.ProductName, cartItem.ProductImageUrl);
        }
    }
}
