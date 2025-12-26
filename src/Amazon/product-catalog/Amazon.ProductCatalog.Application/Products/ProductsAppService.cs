using Amazon.ProductCatalog.Application.Products.Dtos;
using Amazon.ProductCatalog.Domain.Products;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using EMP.SharedKernel;
using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Application.Products;

public class ProductsAppService(
    ProductsService _productsService,
    IRepository<Product, Guid> _productsRepository,
    IUnitOfWork _unitOfWork
)
{
    public async Task<RestResponse<ProductDto>> CreateAsync(CreateProductDto createProductDto)
    {
        var productPrice = new ProductPrice(createProductDto.Price, createProductDto.MinimumPrice, createProductDto.MaximumPrice);

        var createdProductResult = await _productsService.CreateAsync(
            createProductDto.CategoryId,
            createProductDto.Name,
            productPrice
        );
        if (createdProductResult.IsSuccess)
            await _unitOfWork.CommitAsync();

        return createdProductResult.MapTo(MapToDto(createdProductResult.Value));
    }

    public async Task<RestResponse<ProductDto>> GetByIdAsync(Guid productId)
    {
        var product = await _productsRepository.GetByIdAsync(productId);
        if (product is null)
            return RestResponse<ProductDto>.NotFound($"Product with ID {productId} not found");

        return RestResponse<ProductDto>.Success(MapToDto(product));
    }

    public async Task<RestResponse<bool>> UpdateAsync(Guid productId, UpdateProductDto updateProductDto)
    {
        var result = await _productsService.UpdateAsync(productId, updateProductDto.Name, updateProductDto.Price, updateProductDto.Properties);
        if (result.IsSuccess)
            await _unitOfWork.CommitAsync();

        return result;
    }

    public async Task<RestResponse<bool>> DeleteAsync(Guid productId, bool isSoftDelete = true)
    {
        var result = await _productsService.DeleteAsync(productId, isSoftDelete);
        if (result.IsSuccess)
            await _unitOfWork.CommitAsync();

        return result;
    }

    private ProductDto MapToDto(Product product)
    {
        return new ProductDto(
            Id: product.Id,
            CategoryId: product.CategoryId,
            Name: product.Name,
            Price: product.Price.Value,
            Properties: product.Properties.ToList(),
            CreatedAt: product.CreatedOn,
            UpdatedAt: product.UpdatedOn
        );
    }
}
