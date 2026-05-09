using System.Threading.Tasks;
using Moamen.SDKs.Repository;

namespace Amazon.Inventory.Domain.Products;

public class ProductService(IRepository<Product, Guid> repository)
{
}