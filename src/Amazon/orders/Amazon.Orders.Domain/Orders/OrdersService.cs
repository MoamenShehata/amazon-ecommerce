using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
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
        var productsValidationResult = await _productsService.ValidateProducts(cartItems);
        if (!productsValidationResult.IsSuccess)
            return productsValidationResult.MapTo((Order)null);

        //validate customer data

        var order = await _orderFactory.CreateAsync(customerInfo, cartItems);
        _ordersRepo.Add(order);

        return RestResponse<Order>.Created(order, order.Id.ToString());
    }

    public async Task<RestResponse<Order>> GetByIdAsync(Guid id)
    {
        var order = await _ordersRepo.GetInstanceAsync(id);
        if (order is null)
            return RestResponse<Order>.NotFound($"Order ({id}) was not found");

        return RestResponse<Order>.Success(order);
    }

    public async Task<RestResponse<bool>> CancelAsync(Guid orderId)
    {
        var orderResult = await GetByIdAsync(orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return TryCancelOrder(orderResult);
    }

    private RestResponse<bool> TryCancelOrder(Order order)
    {
        if (!order.Status.CanBeCancelled)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot be cancelled!");

        order.Cancel();
        return RestResponse<bool>.Success(true);
    }
}