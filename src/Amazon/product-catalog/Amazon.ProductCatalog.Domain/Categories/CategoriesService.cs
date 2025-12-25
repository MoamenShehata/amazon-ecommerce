using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Categories;

public class CategoriesService(
IRepository<Category, Guid> _categoriesRepository
)
{
    private async Task<bool> DoesCategoryExistByName(string name)
    {
        var categoryBySameName = await _categoriesRepository.FilterSingleAsync(p => p.Name.ToLower() == name.ToLower());
        return categoryBySameName != null;
    }

    public async Task<Category> CreateAsync(string name)
    {
        if (await DoesCategoryExistByName(name))
            throw new Exception();

        var category = new Category(name);
        _categoriesRepository.Add(category);

        return category;
    }

    public async Task UpdateAsync(Guid categoryId, string name, Guid? newParentCategoryId)
    {
        var category = await _categoriesRepository.GetByIdAsync(categoryId)
                ?? throw new Exception();

        if (await DoesCategoryExistByName(name))
            throw new Exception();

        var newParentCategory = newParentCategoryId.HasValue 
            ? await _categoriesRepository.GetByIdAsync(newParentCategoryId.Value)
            : null;

        category.Update(name, newParentCategory);
    }

    /* delete category that has products
    1- either pass new category id to attach orphan products to
    2- either orphans will be soft deleted
    */
    public async Task DeleteAsync(Guid categoryId, Guid? orphanProductsNewCategoryId)
    {
        if (orphanProductsNewCategoryId == categoryId)
            throw new Exception();

        if (orphanProductsNewCategoryId.HasValue)
        {
            _ = await _categoriesRepository.GetByIdAsync(orphanProductsNewCategoryId.Value)
                ?? throw new Exception();
        }

        var category = await _categoriesRepository.GetByIdAsync(categoryId);

        category.SoftDelete(orphanProductsNewCategoryId);
    }
}
