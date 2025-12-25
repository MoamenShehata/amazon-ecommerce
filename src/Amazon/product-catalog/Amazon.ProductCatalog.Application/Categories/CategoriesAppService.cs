using Amazon.ProductCatalog.Application.Categories.Dtos;
using Amazon.ProductCatalog.Domain.Categories;
using EMP.SharedKernel;

namespace Amazon.ProductCatalog.Application.Categories;

public class CategoriesAppService(
    CategoriesService _categoriesService,
    IUnitOfWork _unitOfWork
    )
{
    public async Task<CategoryDto> CreateAsync(string name)
    {
        var createdCategory = await _categoriesService.CreateAsync(name);
        await _unitOfWork.CommitAsync();
        return new CategoryDto(createdCategory.Id, createdCategory.Name);
    }

    public async Task UpdateAsync(Guid categoryId, string newName, Guid? newParentCategoryId)
    {
        await _categoriesService.UpdateAsync(categoryId, newName, newParentCategoryId);
        await _unitOfWork.CommitAsync();
    }

    public async Task DeleteAsync(Guid categoryId, Guid? orphanProductsNewCategoryId)
    {

        await _categoriesService.DeleteAsync(categoryId, orphanProductsNewCategoryId);
        await _unitOfWork.CommitAsync();
    }

}