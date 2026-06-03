using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.Orders.Domain.Products;
using Amazon.Orders.Domain.Stakeholders;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;

namespace Amazon.Orders.Domain.Orders;

public class OrdersService(
    IRepository<Order, Guid> _ordersRepo,
    IRepository<StakeHolder, Guid> _stakeHolders,
    ProductsService _productsService,
    OrderFactory _orderFactory
    )
{
    public async Task<RestResponse<Order>> PlaceOrderAsync(CustomerInfo customerInfo, List<KeyValuePair<Guid, int>> cartItems, object paymentInfo, object deliveryAddress)
    {
        var productsValidationResult = await _productsService.ValidateProducts(cartItems);
        if (!productsValidationResult.IsSuccess)
            return productsValidationResult.MapTo((Order)null);

        //validate customer data

        var order = await _orderFactory.CreateAsync(customerInfo, cartItems, paymentInfo, deliveryAddress);
        _ordersRepo.Add(order);

        return RestResponse<Order>.Created(order, order.Id.ToString());
    }

    public async Task<RestResponse<Order>> GetByUserAsync(Guid requesterUserId, Guid orderId)
    {
        var order = await _ordersRepo.GetInstanceAsync(orderId);
        if (order is null)
            return RestResponse<Order>.NotFound($"Order ({orderId}) was not found");

        var stakeHolder = await _stakeHolders.GetInstanceAsync(requesterUserId);
        if (stakeHolder is null)
            return RestResponse<Order>.NotFound($"User was not found");

        return stakeHolder.CanAccessOrder(order);
    }

    public async Task<RestResponse<bool>> UpdateStatusAsync(Guid requesterUserId, Guid orderId, UpdateOrderStatusRequest request)
    {
        var orderResult = await GetByUserAsync(requesterUserId, orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return orderResult.Value.TryUpdateTo(request.To, request.Payload);
    }

    public async Task<RestResponse<bool>> CancelAsync(Guid requesterUserId, Guid orderId)
    {
        var orderResult = await GetByUserAsync(requesterUserId, orderId);
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