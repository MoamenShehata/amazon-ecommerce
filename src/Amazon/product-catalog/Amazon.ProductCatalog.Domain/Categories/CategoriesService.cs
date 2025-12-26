using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common;
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

    public async Task<RestResponse<Category>> CreateAsync(string name, Guid? parentCategoryId)
    {
        if (await DoesCategoryExistByName(name))
            return RestResponse<Category>.Conflict($"Category with name {name} already exists");

        var parentCategory = parentCategoryId.HasValue
            ? await _categoriesRepository.GetByIdAsync(parentCategoryId.Value)
            : null;

        var category = new Category(name, parentCategory);

        _categoriesRepository.Add(category);

        return RestResponse<Category>.Created(category, category.Id.ToString());
    }

    public async Task<RestResponse<bool>> UpdateAsync(Guid categoryId, string name, Guid? newParentCategoryId)
    {
        var category = await _categoriesRepository.GetByIdAsync(categoryId);
        if (category is null)
            return RestResponse<bool>.NotFound($"Category with id {categoryId} not found");

        if (await DoesCategoryExistByName(name))
            return RestResponse<bool>.Conflict($"Category with name {name} already exists");

        var newParentCategory = newParentCategoryId.HasValue
            ? await _categoriesRepository.GetByIdAsync(newParentCategoryId.Value)
            : null;

        category.Update(name, newParentCategory);
        return RestResponse<bool>.Success(true);
    }

    /* delete category that has products
    1- either pass new category id to attach orphan products to
    2- either orphans will be soft deleted
    */
    public async Task<RestResponse<bool>> DeleteAsync(Guid categoryId, Guid? orphanProductsNewCategoryId)
    {
        if (orphanProductsNewCategoryId == categoryId)
            return RestResponse<bool>.BadRequest(new BadRequestModel("Orphan products new category id cannot be the same as the deleted category id"));

        if (orphanProductsNewCategoryId.HasValue)
        {
            var newCategory = await _categoriesRepository.GetByIdAsync(orphanProductsNewCategoryId.Value);
            if (newCategory is null)
                return RestResponse<bool>.NotFound($"Category with id {orphanProductsNewCategoryId} not found");
        }

        var category = await _categoriesRepository.GetByIdAsync(categoryId);

        category.SoftDelete(orphanProductsNewCategoryId);
        return RestResponse<bool>.Success(true);
    }
}
