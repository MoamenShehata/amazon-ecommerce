using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Categories;

public class CategoriesService(
IRepository<Category, Guid> _categoriesRepository
)
{
    public async Task<Category> CreateAsync(string name)
    {
        var categoryBySameName = await _categoriesRepository.FilterSingleAsync(p => p.Name.ToLower() == name.ToLower());
        if (categoryBySameName != null) throw new Exception();

        var category = new Category(name);
        _categoriesRepository.Add(category);

        return category;
    }
}
