using Amazon.Cart.Domain.Entities;
using Amazon.Cart.Domain.Products;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Services;

public class ProductService(IRepository<Product, Guid> _products)
{
    public async Task<RestResponse<bool>> CreateCartItemAsync(ShoppingCart cart, Guid productId)
    {
        var product = await _products.GetInstanceAsync(x => x.Id == productId && !x.IsDeleted);
        if (product is null)
            return RestResponse<bool>.BadRequest($"Product with id {productId} does not appear in our inventory");

        cart.AddItem(product.CreateCartItem());

        return RestResponse<bool>.Success(true);
    }
}