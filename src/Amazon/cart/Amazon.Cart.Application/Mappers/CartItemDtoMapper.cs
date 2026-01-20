using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Entities;

namespace Amazon.Cart.Application.Mappers
{
    public static class CartItemDtoMapper
    {
        public static List<CartItemDto> ToItemsDto(this ShoppingCart cart)
        {
            return cart.Items.Select(ToDto).ToList();
        }

        public static CartItemDto ToDto(this CartItem cartItem)
        {
            return new CartItemDto(cartItem.ShoppingCartId, cartItem.Id, cartItem.ProductId, cartItem.Quantity, cartItem.ProductName, cartItem.ProductImageUrl);
        }
    }
}