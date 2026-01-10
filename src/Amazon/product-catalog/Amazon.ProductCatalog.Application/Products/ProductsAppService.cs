using Amazon.ProductCatalog.Application.Categories.Dtos;
using Amazon.ProductCatalog.Application.Products.Dtos;
using Amazon.ProductCatalog.Application.Products.Mappers;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.ProductCatalog.Application.Products;

public class ProductsAppService(
    ProductsService _productsService,
    IEfCoreRepository<Product, Guid> _productsRepository,
    IUnitOfWork _unitOfWork
)
{
    public async Task<RestResponse<ProductDto>> CreateAsync(CreateProductDto createProductDto)
    {
        var productPrice = new ProductPrice(createProductDto.Price, createProductDto.MinimumPrice, createProductDto.MaximumPrice);

        var createdProductResult = await _productsService.CreateAsync(
            createProductDto.CategoryId,
            createProductDto.Name,
            productPrice,
            createProductDto.Properties
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

    public async Task<RestResponse<bool>> DeleteAsync(Guid productId, bool isSoftDelete = true)
    {
        var result = await _productsService.DeleteAsync(productId, isSoftDelete);
        if (result.IsSuccess)
            await _unitOfWork.CommitAsync();

        return result;
    }
}
