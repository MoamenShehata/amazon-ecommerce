using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.Cart.Domain.ShoppingCarts.Entites;

namespace Amazon.Cart.Application.Mappers;

public static class ShoppingCartIMapper
{
    public static List<CartItemDto> ToItemsDto(this ShoppingCart cart, IEnumerable<Product> products)
    {
        var dto = new List<CartItemDto>();
        foreach (var cartItem in cart.Items)
        {
            var product = products.FirstOrDefault(x => x.Id == cartItem.ProductId);
            dto.Add(new CartItemDto(cartItem.ProductId, cartItem.Info.Name, cartItem.Info.ImageUrl, cartItem.Quantity, cartItem.Info.UnitPrice, product != null && !product.IsDeleted));
        }
        return dto;
    }
}