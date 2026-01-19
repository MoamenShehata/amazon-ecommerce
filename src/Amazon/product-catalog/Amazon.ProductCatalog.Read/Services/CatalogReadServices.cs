using Amazon.ProductCatalog.Read.Models;
using Amazon.SharedKernel.Common;
using Moamen.SDKs.Repository.Pagination;

namespace Amazon.ProductCatalog.Read.Services
{
    public interface ICatalogReadServices
    {
        Task InsertProductAsync(Guid id, string Name, string Categories, decimal UnitPrice);
        Task<PagedResult<ProductForListModel, DateTime>> GetProductsPageAsync(PageRequest pageRequest);
        Task UpdateImagePathAsync(Guid productId, string imagePath);
    }
}
