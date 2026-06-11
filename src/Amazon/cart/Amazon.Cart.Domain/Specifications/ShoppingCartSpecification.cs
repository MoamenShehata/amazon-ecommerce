using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Specifications;

public class ShoppingCartSpecification(
        IRepository<Product, Guid> _products
    )
{
    public async Task<RestResponse<bool>> SatisfiesAsync(ShoppingCart cart)
    {
        var products = await _products.GetAllAsync(p => cart.AggregatToProducts.Select(x => x.Key).Contains(p.Id));

        foreach (var cartProduct in cart.AggregatToProducts)
        {
            var systemProduct = products.FirstOrDefault(x => x.Id == cartProduct.Key);
            if (systemProduct.IsDeleted)
                return RestResponse<bool>.BadRequest($"Shopping cart is in invalid statues, product named {systemProduct.Info.Name} is not in our system");
        }

        return RestResponse<bool>.Success(true);
    }
}