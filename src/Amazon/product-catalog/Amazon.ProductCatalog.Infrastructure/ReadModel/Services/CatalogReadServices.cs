using Amazon.ProductCatalog.Read.Models;
using Amazon.ProductCatalog.Read.Services;
using Amazon.SharedKernel.Common;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;

namespace Amazon.ProductCatalog.Infrastructure.ReadModel.Services;

public class CatalogReadServices : ICatalogReadServices
{
    private readonly CatalogReadContext _readContext;
    private readonly EfCoreRepositoryBase<ProductForListModel, Guid> _productsRepository;

    public CatalogReadServices(CatalogReadContext readContext)
    {
        _readContext = readContext;
        _productsRepository = new EfCoreRepositoryBase<ProductForListModel, Guid>(_readContext);
    }

    public async Task<PagedResult<ProductForListModel, DateTime>> GetProductsPageAsync(PageRequest pageRequest)
    {
        var page = pageRequest.PageNumber == 1
            ? await _productsRepository.GetPageAsync(new PagedRequest(pageRequest.PageNumber, pageRequest.PageSize), c => c.CreatedOn)
            : await _productsRepository.GetPageAsync(pageRequest.PageSize, c => c.CreatedOn, (DateTime)pageRequest.LastSeenValue);

        return new PagedResult<ProductForListModel, DateTime>(page.Items, page.TotalCount, page.LastSeenValue);
    }

    public async Task InsertProductAsync(Guid id, string name, string categories, decimal unitPrice)
    {
        var exists = await _productsRepository.ExistsAsync(id);
        if (exists) return;

        _productsRepository.Add(new ProductForListModel(id, name, categories, unitPrice));
        await _readContext.SaveChangesAsync();
    }
}
