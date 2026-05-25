using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain;

namespace Amazon.Cart.Application.Mappers
{
    public static class CartItemDtoMapper
    {
        public static List<CartItemDto> ToItemsDto(this ShoppingCart cart)
        {
            var productGroups = cart.Items.GroupBy(x => x.ProductId);

            var dtos = new List<CartItemDto>();
            foreach (var productGroup in productGroups)
            {
                var productInfo = cart.Items.FirstOrDefault(x => x.ProductId == productGroup.Key).Product.Info;

                dtos.Add(new CartItemDto(productGroup.Key, productInfo.Name, productInfo.ImageUrl, cart.Items.Where(i => i.ProductId == productGroup.Key).Select(x => x.Id).ToList()));
            }

            return dtos;
        }
    }
}