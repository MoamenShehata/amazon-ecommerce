using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;

namespace Amazon.Cart.Domain.Specifications;

public class ShoppingCartSpecification(
        IProductsRepo _products
    )
{
    public async Task<RestResponse<bool>> SatisfiesAsync(ShoppingCart cart)
    {
        var products = await _products.GetCartProductsAsync(cart);

        foreach (var cartItem in cart.Items)
        {
            var systemProduct = products.FirstOrDefault(x => x.Id == cartItem.ProductId);
            if (systemProduct is null || systemProduct.IsDeleted)
                return RestResponse<bool>.BadRequest($"Shopping cart is in invalid statues, product named {systemProduct.Info.Name} is not in our system");
        }

        return RestResponse<bool>.Success(true);
    }
}