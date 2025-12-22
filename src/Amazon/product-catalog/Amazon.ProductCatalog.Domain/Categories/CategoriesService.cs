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

    /* delete category that has products
    1- either pass new category id to attach orphan products to
    2- either orphans will be soft deleted
    */
    public async Task DeleteAsync(Guid categoryId, Guid? orphanProductsNewCategoryId)
    {
        if (orphanProductsNewCategoryId.HasValue)
        {
            var orphanProductsNewCategory = await _categoriesRepository.GetByIdAsync(orphanProductsNewCategoryId.Value)
                ?? throw new Exception();
        }

        var category = await _categoriesRepository.GetByIdAsync(categoryId);

        category.SoftDelete(orphanProductsNewCategoryId);
    }
}
