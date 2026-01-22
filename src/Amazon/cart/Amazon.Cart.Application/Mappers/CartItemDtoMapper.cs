using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain;

namespace Amazon.Cart.Application.Mappers
{
    public static class CartItemDtoMapper
    {
        public static List<CartProductDto> ToItemsDto(this ShoppingCart cart)
        {
            var productGroups = cart.Items.GroupBy(x => x.ProductId);

            var dtos = new List<CartProductDto>();
            foreach (var productGroup in productGroups)
            {
                var productData = cart.Items.FirstOrDefault(x => x.ProductId == productGroup.Key);

                dtos.Add(new CartProductDto(productGroup.Key, productData.ProductName, productData.ProductImageUrl, cart.Items.Where(i => i.ProductId == productGroup.Key).Select(x => x.Id).ToList()));
            }

            return dtos;
        }
    }
}