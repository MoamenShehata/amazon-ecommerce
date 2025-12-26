using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Common;
using EMP.SharedKernel;
using EMP.SharedKernel.Repositories;

namespace Amazon.ProductCatalog.Domain.Products;

public class ProductsService(
    IRepository<Category, Guid> _categoriesRepository,
    IRepository<Product, Guid> _productsRepository
    )
{
    public async Task<RestResponse<Product>> CreateAsync(Guid categoryId, string name, ProductPrice price)
    {
        var category = await _categoriesRepository.GetByIdAsync(categoryId);
        if (category is null)
            return RestResponse<Product>.NotFound($"Category with id {categoryId} not found");

        var product = category.NewProduct(name, price);
        _productsRepository.Add(product);

        return RestResponse<Product>.Created(product);
    }

    public async Task<RestResponse<bool>> UpdateAsync(Guid id, string newName, decimal productPrice, IEnumerable<ProductProperty> properties)
    {
        var existingProduct = await _productsRepository.GetByIdAsync(id);
        if (existingProduct is null)
            return RestResponse<bool>.NotFound($"Product with id {id} not found");

        var updateResult = existingProduct.UpdateFrom(newName, productPrice, properties);

        return RestResponse<bool>.Success(true);
    }

    public async Task<RestResponse<bool>> DeleteAsync(Guid productId, bool isSoftDelete)
    {
        var existingProduct = await _productsRepository.GetByIdAsync(productId);
        if (existingProduct is null)
            return RestResponse<bool>.NotFound($"Product with id {productId} not found");

        if (isSoftDelete)
            existingProduct.SoftDelete();
        else
            _productsRepository.Remove(existingProduct);

        return RestResponse<bool>.Success(true);
    }
}