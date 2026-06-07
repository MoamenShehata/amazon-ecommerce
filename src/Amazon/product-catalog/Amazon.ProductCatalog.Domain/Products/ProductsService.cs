using Amazon.ProductCatalog.Domain.Categories;
using Amazon.ProductCatalog.Domain.Products.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Media;
using Amazon.SharedKernel.Media.Events;
using Amazon.SharedKernel.Products.Events;
using Moamen.SDKs.Repository;

namespace Amazon.ProductCatalog.Domain.Products;

public class ProductsService(
    IEfCoreRepository<Category, Guid> _categoriesRepository,
    CategoriesService _categoriesService,
    IEfCoreRepository<Product, Guid> _productsRepository,
    IMediaService _mediaService
    )
{
    public async Task<RestResponse<(Product, Category)>> CreateAsync(Guid categoryId, string name, int inStockCount, ProductPrice price, List<ProductProperty> properties, MediaContent mediaUploadRequest)
    {
        var categoryResult = await _categoriesService.GetByIdAsync(categoryId, true);
        if (categoryResult is null)
            return RestResponse<(Product, Category)>.NotFound($"Category with id {categoryId} not found");


        var imageUrl = await _mediaService.UploadFileAsync(mediaUploadRequest);
        var product = categoryResult.Value.NewProduct(name, imageUrl, price, inStockCount, properties);

        product.RaiseEvent(new ProductCreatedEvent(categoryId, product.Id, product.Name, inStockCount, product.Price.Amount, categoryResult.Value.FullName, imageUrl));
        product.RaiseEvent(new MediaCreateRequestedEvent(product.Id, product.Id, mediaUploadRequest, true));

        _productsRepository.Add(product);

        return RestResponse<(Product, Category)>.Created((product, categoryResult), product.Id.ToString());
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
        product.RaiseEvent(new ProductDeletedEvent(product.Id));

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