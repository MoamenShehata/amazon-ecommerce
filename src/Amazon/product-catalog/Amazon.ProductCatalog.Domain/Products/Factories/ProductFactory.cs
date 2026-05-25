//using Amazon.ProductCatalog.Domain.Categories;
//using Amazon.ProductCatalog.Domain.Products.ValueObjects;
//using Amazon.SharedKernel.API;
//using Amazon.SharedKernel.Media;
//using Moamen.SDKs.Repository;

//namespace Amazon.ProductCatalog.Domain.Products.Factories;

//public class ProductFactory(
//    IEfCoreRepository<Category, Guid> _categoriesRepository,
//    CategoriesService _categoriesService,
//    IEfCoreRepository<Product, Guid> _productsRepository,
//    IMediaService _mediaService
//    )
//{
//    public async Task<Product> CreateAsync(Guid categoryId, string name, int inStockCount, ProductPrice price, List<ProductProperty> properties, MediaContent mediaUploadRequest)
//    {
//        var categoryResult = await _categoriesService.GetByIdAsync(categoryId, true);
//        if (categoryResult is null)
//            return RestResponse<(Product, Category)>.NotFound($"Category with id {categoryId} not found");


//        var imageUrl = await _mediaService.UploadFileAsync(mediaUploadRequest);
//        var product = categoryResult.Value.NewProduct(name, imageUrl, price, properties);
//    }
//}