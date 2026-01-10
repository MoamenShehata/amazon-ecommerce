using Amazon.ProductCatalog.Application.Categories.Dtos;
using Amazon.ProductCatalog.Application.Categories.Mappers;
using Amazon.ProductCatalog.Application.Common.Dtos;
using Amazon.ProductCatalog.Domain.Categories;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;
using Moamen.SDKs.SharedKernel;

namespace Amazon.ProductCatalog.Application.Categories;

public class CategoriesAppService(
    CategoriesService _categoriesService,
    IEfCoreRepository<Category, Guid> _categoriesRepository,
    IUnitOfWork _unitOfWork
    )
{
    public async Task<PagedResult<CategoryForListDto, DateTime>> GetPageAsync(PageRequest pageRequest)
    {
        var page = pageRequest.PageNumber == 1
            ? await _categoriesRepository.GetPageAsync(new PagedRequest(pageRequest.PageNumber, pageRequest.PageSize), c => c.CreatedOn)
            : await _categoriesRepository.GetPageAsync(pageRequest.PageSize, c => c.CreatedOn, (DateTime)pageRequest.LastSeenValue);

        return new PagedResult<CategoryForListDto, DateTime>(page.Items.Select(x => x.ToDtoForList()), page.TotalCount, page.LastSeenValue);
    }

    public async Task<RestResponse<CategoryDto>> GetByIdAsync(Guid id)
    {
        var categoryResult = await _categoriesService.GetByIdAsync(id, true);
        if (!categoryResult.IsSuccess)
            return categoryResult.MapTo((CategoryDto)null);

        return categoryResult.MapTo(categoryResult.Value.ToDto());
    }

    public async Task<RestResponse<CategoryDto>> CreateAsync(CreateCategoryRequest request)
    {
        var result = await _categoriesService.CreateAsync(request.Name, request.ParentCategoryId);
        if (!result.IsSuccess)
            return result.MapTo((CategoryDto)null);

        await _unitOfWork.CommitAsync();

        return result.MapTo(new CategoryDto(result.Value.Id, result.Value.Name));
    }

    public async Task<RestResponse<bool>> UpdateAsync(Guid categoryId, UpdateCategoryRequest updateCategoryRequest)
    {
        var result = await _categoriesService.UpdateAsync(categoryId, updateCategoryRequest.Name, updateCategoryRequest.NewParentCategoryId);
        if (result.IsSuccess)
            await _unitOfWork.CommitAsync();

        return result;
    }

    public async Task<RestResponse<bool>> DeleteAsync(Guid categoryId, Guid? orphanProductsNewCategoryId)
    {
        var result = await _categoriesService.DeleteAsync(categoryId, orphanProductsNewCategoryId);
        if (result.IsSuccess)
            await _unitOfWork.CommitAsync();

        return result;
    }
}