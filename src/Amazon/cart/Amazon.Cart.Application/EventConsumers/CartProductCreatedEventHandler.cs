using Amazon.Cart.Domain.Products;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Application.EventConsumers;

public class CartProductCreatedEventHandler(
IRepository<Product, Guid> _productsRepo,
IUnitOfWork _unitOfWork) : IConsumer<ProductCreatedEvent>
{
    public async Task Consume(ConsumeContext<ProductCreatedEvent> context)
    {
        var productEvent = context.Message;

        var product = new Product(productEvent.ProductId, new Domain.Products.ValueObjects.ProductInfo(productEvent.Name, productEvent.ImageUrl));
        _productsRepo.Add(product);

        await _unitOfWork.CommitAsync();
    }
}
