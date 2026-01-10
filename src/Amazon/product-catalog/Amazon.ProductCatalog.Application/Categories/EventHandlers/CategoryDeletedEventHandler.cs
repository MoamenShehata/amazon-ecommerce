//using Amazon.ProductCatalog.Domain.Categories;
//using Amazon.ProductCatalog.Domain.Categories.Events;
//using Amazon.ProductCatalog.Domain.Products;
//using MediatR;
//using Moamen.SDKs.Repository;
//using Moamen.SDKs.SharedKernel;

//namespace Amazon.ProductCatalog.Application.Categories.EventHandlers;

////simulation of Eventual consistency (that run in-process)
//public class CategorySoftDeletedEventHandler(
//    IRepository<Category, Guid> _categoriesRepository,
//    IRepository<Product, Guid> _productsRepository,
//    IUnitOfWork _unitOfWork
//    )
//    : INotificationHandler<CategorySoftDeletedEvent>
//{
//    public async Task Handle(CategorySoftDeletedEvent notification, CancellationToken cancellationToken)
//    {
//        var orphanProducts = await _productsRepository.GetAllAsync(p => p.CategoryId == notification.CategoryId);
//        if (notification.OrphanProductsNewCategoryId.HasValue)
//        {
//            foreach (var product in orphanProducts)
//                product.AttachToCategory(notification.OrphanProductsNewCategoryId.Value);
//        }
//        else
//        {
//            foreach (var product in orphanProducts)
//                product.SoftDelete();
//        }

//        await _unitOfWork.CommitAsync();
//    }
//}
