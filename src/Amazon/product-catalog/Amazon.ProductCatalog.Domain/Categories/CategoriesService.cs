using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Categories;

public class CategoriesService(
IRepository<Category, Guid> _categoriesRepository
)
{
    public async Task<Category> CreateAsync(string name, ProductPrice price)
    {
        var categoryBySameName = await _categoriesRepository.FilterSingleAsync(p => p.Name.ToLower() == name.ToLower());
        if (categoryBySameName != null) throw new Exception();

        return new(name);
    }
}
