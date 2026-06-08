using Amazon.Cart.Application.Dtos;
using Amazon.Cart.Domain;
using Amazon.Cart.Domain.Products;

namespace Amazon.Cart.Application.Mappers
{
    public static class CartItemDtoMapper
    {
        public static List<CartItemDto> ToItemsDto(this ShoppingCart cart, IEnumerable<Product> products)
        {
            var productGroups = cart.Items.GroupBy(x => x.ProductId);

            var dtos = new List<CartItemDto>();
            foreach (var productGroup in productGroups)
            {
                var product = products.FirstOrDefault(x => x.Id == productGroup.Key);
                var productInfo = product.Info;

                dtos.Add(new CartItemDto(productGroup.Key, productInfo.Name, productInfo.ImageUrl, cart.Items.Where(i => i.ProductId == productGroup.Key).Select(x => 0).ToList(), productInfo.UnitPrice, !product.IsDeleted));
            }

            return dtos;
        }
    }
}