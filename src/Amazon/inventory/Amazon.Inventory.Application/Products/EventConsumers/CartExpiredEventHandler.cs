using Amazon.SharedKernel.IntegrationEvents.ShoppingCart;
using MassTransit;

namespace Amazon.Inventory.Application.Products.EventConsumers;

public class CartExpiredEventHandler(
    ProductAppService productService) : IConsumer<CartExpiredEvent>
{
    public async Task Consume(ConsumeContext<CartExpiredEvent> context)
    {
        await productService.ReleaseProductsOnHoldAsync(context.Message.ProductIds);
    }
}