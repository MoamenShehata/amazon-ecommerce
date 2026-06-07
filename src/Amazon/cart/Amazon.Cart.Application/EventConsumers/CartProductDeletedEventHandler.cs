using Amazon.Cart.Domain.Products;
using Amazon.Cart.Domain.Products.ValueObjects;
using Amazon.SharedKernel.Products.Events;
using MassTransit;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Cart.Application.EventConsumers;

public class CartProductDeletedEventHandler(
IRepository<Product, Guid> _productsRepo,
IUnitOfWork _unitOfWork) : IConsumer<ProductDeletedEvent>
{
    public async Task Consume(ConsumeContext<ProductDeletedEvent> context)
    {
        var productEvent = context.Message;

        var product = await _productsRepo.GetInstanceAsync(productEvent.ProductId);
        _productsRepo.Remove(product);

        // should delete linked cart items too

        await _unitOfWork.CommitAsync();
    }
}
