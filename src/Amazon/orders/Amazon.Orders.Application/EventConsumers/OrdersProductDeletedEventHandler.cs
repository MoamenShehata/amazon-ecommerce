using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Orders.Application.EventConsumers;

public class OrdersProductDeletedEventHandler(
IRepository<Product, Guid> _productsRepo,
IUnitOfWork _unitOfWork) : IConsumer<ProductDeletedEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        var productEvent = context.Message;

        var product = await _productsRepo.GetInstanceAsync(productEvent.ProductId);
        _productsRepo.Remove(product);

        await _unitOfWork.CommitAsync();
    }
}