using Amazon.SharedKernel.API;
using Microsoft.EntityFrameworkCore;
using Moamen.SDKs.Repository;

namespace Amazon.ProductCatalog.Domain.Categories;

public class CategoriesService(
IEfCoreRepository<Category, Guid> _categoriesRepository
)
{
    private async Task<bool> DoesCategoryExistByName(string name)
    {
        var categoryBySameName = await _categoriesRepository.GetInstanceAsync(p => p.Name.ToLower() == name.ToLower());
        return categoryBySameName != null;
    }

    public async Task<RestResponse<Category>> CreateAsync(string name, Guid? parentCategoryId)
    {
        if (await DoesCategoryExistByName(name))
            return RestResponse<Category>.Conflict($"Category with name {name} already exists");

        var parentCategory = parentCategoryId.HasValue
            ? await _categoriesRepository.GetInstanceAsync(parentCategoryId.Value)
            : null;

        var category = new Category(name, parentCategory);

        _categoriesRepository.Add(category);

        return RestResponse<Category>.Created(category, category.Id.ToString());
    }

    public async Task<RestResponse<Category>> GetByIdAsync(Guid categoryId, bool includeRelations = false)
    {
        var categoryById = includeRelations ?
            await _categoriesRepository.GetInstanceAsync(categoryId, c => c.Include(x => x.ParentCategory).Include(x => x.Children))
            : await _categoriesRepository.GetInstanceAsync(categoryId);

        if (categoryById == null) return RestResponse<Category>.NotFound($"Category with id {categoryId} not found");

        return RestResponse<Category>.Success(categoryById);
    }

    public async Task<RestResponse<bool>> UpdateAsync(Guid categoryId, string name, Guid? newParentCategoryId)
    {
        var categoryResult = await GetByIdAsync(categoryId);
        if (!categoryResult.IsSuccess)
            return RestResponse<bool>.NotFound(categoryResult.Error!);

        if (await DoesCategoryExistByName(name))
            return RestResponse<bool>.Conflict($"Category with name {name} already exists");

        var newParentCategory = newParentCategoryId.HasValue
            ? await _categoriesRepository.GetInstanceAsync(newParentCategoryId.Value)
            : null;

        categoryResult.Value.Update(name, newParentCategory);
        return RestResponse<bool>.Success(true);
    }

    /* delete categoryResult that has products
    1- either pass new categoryResult id to attach orphan products to
    2- either orphans will be soft deleted
    */
    public async Task<RestResponse<bool>> DeleteAsync(Guid categoryId, Guid? orphanProductsNewCategoryId)
    {
        if (orphanProductsNewCategoryId == categoryId)
            return RestResponse<bool>.BadRequest(new BadRequestModel("Orphan products new category id cannot be the same as the deleted category id"));

        if (orphanProductsNewCategoryId.HasValue)
        {
            var newCategory = await _categoriesRepository.GetInstanceAsync(orphanProductsNewCategoryId.Value);
            if (newCategory is null)
                return RestResponse<bool>.NotFound($"Category with id {orphanProductsNewCategoryId} not found");
        }

        var category = await _categoriesRepository.GetInstanceAsync(categoryId);

        category.SoftDelete(orphanProductsNewCategoryId);
        return RestResponse<bool>.Success(true);
    }
}
