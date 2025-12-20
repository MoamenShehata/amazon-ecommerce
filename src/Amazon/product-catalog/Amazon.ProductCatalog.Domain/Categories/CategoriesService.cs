using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using EMP.SharedKernel;
using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Categories;

public class CategoriesService(
IRepository<Category, Guid> _categoriesRepository,
IUnitOfWork _unitOfWork
)
{
    public async Task<Category> CreateAsync(string name, ProductPrice price)
    {
        var categoryBySameName = await _categoriesRepository.FilterSingleAsync(p => p.Name.ToLower() == name.ToLower());
        if (categoryBySameName != null) throw new Exception();

        var category = new Category(name);
        _categoriesRepository.Add(category);

        await _unitOfWork.CommitAsync();
        return category;
    }
}
