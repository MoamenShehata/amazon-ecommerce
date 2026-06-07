using Amazon.ProductCatalog.Application.Products.Dtos;
using Amazon.ProductCatalog.Application.Products.Mappers;
using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common;
using Amazon.SharedKernel.Extensions;
using Amazon.SharedKernel.Media;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;
using Moamen.SDKs.SharedKernel;

namespace Amazon.ProductCatalog.Application.Products;

public class ProductsAppService(
    ProductsService _productsService,
    IEfCoreRepository<Product, Guid> _productsRepository,
    IEfCoreRepository<Category, Guid> _categroiesRepository,
    IUnitOfWork _unitOfWork
    )
{
    public async Task<PagedResult<ProductForListDto, DateTime>> GetProductsPageAsync(PageRequest pageRequest)
    {
        var page = pageRequest.PageNumber == 1
            ? await _productsRepository.GetPageAsync(new PagedRequest(pageRequest.PageNumber, pageRequest.PageSize), c => c.CreatedOn)
            : await _productsRepository.GetPageAsync(pageRequest.PageSize, c => c.CreatedOn, DateTime.Parse(pageRequest.LastSeenValue));

        List<ProductForListDto> dto = new();
        var categories = await _categroiesRepository.GetAllAsync(x => page.Items.Select(p => p.CategoryId).Contains(x.Id));

        foreach (var product in page.Items)
            dto.Add(new ProductForListDto(product.Id, product.Name, categories.FirstOrDefault(c => c.Id == product.CategoryId).FullName, product.Price.Amount, product.ImageUrl, product.IsAvailableInInventory));

        return new PagedResult<ProductForListDto, DateTime>(dto, page.TotalCount, page.LastSeenValue);
    }

    public async Task<RestResponse<ProductDto>> CreateAsync(CreateProductDto createProductDto,
        MediaContent mediaUploadRequest)
    {
        var productPrice = new ProductPrice(createProductDto.Price, createProductDto.MinimumPrice, createProductDto.MaximumPrice);

        var createdProductResult = await _productsService.CreateAsync(
            createProductDto.CategoryId,
            createProductDto.Name,
            createProductDto.InStockCount,
            productPrice,
            createProductDto.Properties.Select(p => new ProductProperty(p.Key, p.Value)).ToList(),
            mediaUploadRequest
        );
        if (!createdProductResult.IsSuccess)
            return createdProductResult.MapTo((ProductDto)null!);

        await _unitOfWork.CommitAsync();

        return createdProductResult.MapTo(createdProductResult.Value.ToDto());
    }

    public async Task<RestResponse<ProductDto>> GetByIdAsync(Guid productId)
    {
        var productModelResult = await _productsService.GetWithCategoryByIdAsync(productId);
        if (!productModelResult.IsSuccess)
            return RestResponse<ProductDto>.NotFound(productModelResult.Error!);

        return RestResponse<ProductDto>.Success(productModelResult.Value.ToDto());
    }

    public async Task<RestResponse<bool>> UpdateAsync(Guid productId, UpdateProductDto updateProductDto)
    {
        var productModelResult = await _productsService.GetByIdAsync(productId);
        if (!productModelResult.IsSuccess)
            return RestResponse<bool>.NotFound(productModelResult.Error!);

        var updateResult = productModelResult.Value.UpdateFrom(updateProductDto.Name, updateProductDto.Price, updateProductDto.Properties);
        await _unitOfWork.CommitAsync();

        return RestResponse<bool>.Success(true);
    }

    public async Task UpdateImagePathAsync(Guid ownerId, string url)
    {
        var productResult = await _productsService.GetByIdAsync(ownerId);
        if (!productResult.IsSuccess)
            return;

        productResult.Value.UpdateImageUrl(url);
        await _unitOfWork.CommitAsync();
    }

    public async Task<RestResponse<bool>> DeleteAsync(Guid productId, bool isSoftDelete = true)
    {
        var result = await _productsService.DeleteAsync(productId, false);
        if (result.IsSuccess)
            await _unitOfWork.CommitAsync();

        return result;
    }
}
