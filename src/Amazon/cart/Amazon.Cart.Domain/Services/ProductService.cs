using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.ShoppingCarts;
using Amazon.Cart.Domain.ShoppingCarts.Entites;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Services;

public class ProductService(IRepository<Product, Guid> _products)
{
    public async Task<RestResponse<CartItem>> CreateCartItemAsync(ShoppingCart cart, Guid productId)
    {
        // should come from cache if valid
        var product = await _products.GetInstanceAsync(x => x.Id == productId && !x.IsDeleted);
        if (product is null)
            return RestResponse<CartItem>.BadRequest($"Product with id {productId} does not appear in our inventory");

        var cartItem = cart.PushProductItem(product);
        return RestResponse<CartItem>.Success(cartItem);
    }
}