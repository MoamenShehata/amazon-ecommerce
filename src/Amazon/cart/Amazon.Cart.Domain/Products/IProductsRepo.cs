using Amazon.Cart.Domain.ShoppingCarts;
using Moamen.SDKs.Repository;

namespace Amazon.Cart.Domain.Products;

public interface IProductsRepo : IRepository<Product, Guid>
{
    Task<List<Product>> GetCartProductsAsync(ShoppingCart shoppingCart);
}