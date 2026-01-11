using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.API;
using Moamen.SDKs.Repository;

namespace Amazon.Orders.Domain.Orders;

public class OrdersService(
    IRepository<Order, Guid> _ordersRepo,
    ProductsService _productsService,
    OrderFactory _orderFactory
    )
{
    public async Task<RestResponse<Order>> PlaceOrderAsync(CustomerInfo customerInfo, List<KeyValuePair<Guid, int>> cartItems)
    {
        var productsValidationResult = await _productsService.ValidateProducts(cartItems.Select(x => x.Key).ToList());
        if (!productsValidationResult.IsSuccess)
            return RestResponse<Order>.Failure(productsValidationResult.Error.ToString());

        var order = await _orderFactory.CreateAsync(customerInfo, cartItems);
        _ordersRepo.Add(order);

        return RestResponse<Order>.Created(order, order.Id.ToString());
    }
}