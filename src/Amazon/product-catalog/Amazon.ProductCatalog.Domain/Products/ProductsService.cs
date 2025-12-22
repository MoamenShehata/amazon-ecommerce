using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using EMP.SharedKernel;
using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Products;

public class ProductsService(
    IRepository<Category, Guid> _categoriesRepository,
    IRepository<Product, Guid> _productsRepository
    )
{
    public async Task<Product> CreateAsync(Guid categoryId, string name, ProductPrice price)
    {
        var category = await _categoriesRepository.GetByIdAsync(categoryId) ?? throw new Exception();

        var product = category.NewProduct(name, price);
        _productsRepository.Add(product);

        return product;
    }

    public async Task<ApiResult<bool>> UpdateAsync(Product productNewVersion)
    {
        var existingProduct = await _productsRepository.GetByIdAsync(productNewVersion.Id);
        if (existingProduct is null) throw new Exception();

        var updateResult = existingProduct.UpdateFrom(productNewVersion);
        if (!updateResult.Success)
            return updateResult;

        return ApiResponseExtentions.Success(true);
    }

    public async Task DeleteAsync(Guid productId, bool isSoftDelete)
    {
        var product = await _productsRepository.GetByIdAsync(productId);

        if (isSoftDelete)
            product.SoftDelete();
        else
            _productsRepository.Remove(product);
    }
}