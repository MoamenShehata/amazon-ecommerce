using System.Xml.Linq;
using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products.Events;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using MediatR;
using Moamen.SDKs.Repository;

namespace Amazon.ProductCatalog.Domain.Products;

public class ProductsService(
    IEfCoreRepository<Category, Guid> _categoriesRepository,
    IEfCoreRepository<Product, Guid> _productsRepository
    )
{
    public async Task<RestResponse<(Product, Category)>> CreateAsync(Guid categoryId, string name, int inStockCount, ProductPrice price, List<ProductProperty> properties)
    {
        var category = await _categoriesRepository.GetInstanceAsync(categoryId);
        if (category is null)
            return RestResponse<(Product, Category)>.NotFound($"Category with id {categoryId} not found");

        var product = category.NewProduct(name, price, properties);
        product.RaiseEvent(new ProductCreatedEvent(categoryId, product.Id, product.Name, inStockCount, product.Price.Amount));

        _productsRepository.Add(product);

        return RestResponse<(Product, Category)>.Created((product, category), product.Id.ToString());
    }

    public async Task<RestResponse<(Product, Category)>> GetWithCategoryByIdAsync(Guid productId)
    {
        var productByIdResult = await GetByIdAsync(productId);
        if (!productByIdResult.IsSuccess)
            return RestResponse<(Product, Category)>.NotFound(productByIdResult.Error!);

        var category = await _categoriesRepository.GetInstanceAsync(productByIdResult.Value.CategoryId);

        return RestResponse<(Product, Category)>.Success((productByIdResult, category));
    }

    public async Task<RestResponse<Product>> GetByIdAsync(Guid productId)
    {
        var existingProduct = await _productsRepository.GetInstanceAsync(productId);
        if (existingProduct is null)
            return RestResponse<Product>.NotFound($"Product with id {productId} not found");

        return RestResponse<Product>.Success(existingProduct);
    }

    public async Task<RestResponse<bool>> DeleteAsync(Guid productId, bool isSoftDelete)
    {
        var productByIdResult = await GetByIdAsync(productId);
        if (!productByIdResult.IsSuccess)
            return RestResponse<bool>.NotFound(productByIdResult.Error!);

        return HandleDelete(productByIdResult, isSoftDelete);
    }

    private RestResponse<bool> HandleDelete(Product product, bool isSoftDelete)
    {
        if (isSoftDelete)
            product.SoftDelete();
        else
            _productsRepository.Remove(product);

        return RestResponse<bool>.Success(true);
    }

    public async Task ReAttachOrphanProductsForDeletedCategory(Guid deletedCategoryId, Guid? newCategoryIdToReattach)
    {
        var orphanProducts = await _productsRepository.GetAllAsync(p => p.CategoryId == deletedCategoryId);
        Action<Product> deleteAction = newCategoryIdToReattach.HasValue
            ? p => p.AttachToCategory(newCategoryIdToReattach.Value)
            : p => p.SoftDelete();

        foreach (var product in orphanProducts)
            deleteAction(product);
    }
}