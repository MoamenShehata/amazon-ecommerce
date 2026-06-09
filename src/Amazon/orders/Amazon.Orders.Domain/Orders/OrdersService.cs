using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Amazon.Orders.Domain.Stakeholders;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Customers;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using Moamen.SDKs.Repository.Pagination;
using System.Linq.Expressions;

namespace Amazon.Orders.Domain.Orders;

public class OrdersService(
    IRepository<Order, Guid> _orders,
    IRepository<StakeHolder, Guid> _stakeHolders,
    ProductsService _productsService,
    OrderFactory _orderFactory
    )
{
    public async Task<RestResponse<PagedResult<Order, DateTime>>> GetOrdersPageByRequestingCustomer(Guid requesterCustomerId, int pageNumber, int pageSize, DateTime? lastSeenValue)
    {
        var stakeHolder = await _stakeHolders.GetInstanceAsync(requesterCustomerId);
        if (stakeHolder is null)
            return RestResponse<PagedResult<Order, DateTime>>.NotFound($"Customer was not found");

        var filters = new List<Expression<Func<Order, bool>>>();

        if (stakeHolder is DeliveryUser)
            return RestResponse<PagedResult<Order, DateTime>>.BadRequest($"Customer not authorized to access this area!");

        if (stakeHolder is Customer)
            filters.Add(x => x.Owner.Id == requesterCustomerId);

        var page = pageNumber == 1
        ? await _orders.GetPageAsync(new PagedRequest(pageNumber, pageSize), c => c.CreatedOn, filters)
        : await _orders.GetPageAsync(pageSize, c => c.CreatedOn, lastSeenValue.Value, filters);

        return RestResponse<PagedResult<Order, DateTime>>.Success(new PagedResult<Order, DateTime>(page.Items, page.TotalCount, page.LastSeenValue));
    }

    public async Task<RestResponse<Order>> PlaceOrderAsync(Guid orderId, CustomerInfo customerInfo, List<KeyValuePair<Guid, int>> cartItems, DeliveryAddress deliveryAddress)
    {
        //validate customer data

        var order = await _orderFactory.CreateAsync(orderId, customerInfo, cartItems, deliveryAddress);
        _orders.Add(order);

        return RestResponse<Order>.Created(order, order.Id.ToString());
    }

    public async Task<RestResponse<Order>> GetByUserForReadAsync(Guid requesterUserId, Guid orderId)
    {
        var order = await _orders.GetInstanceAsync(orderId);
        if (order is null)
            return RestResponse<Order>.NotFound($"Order ({orderId}) was not found");

        var stakeHolder = await _stakeHolders.GetInstanceAsync(requesterUserId);
        if (stakeHolder is null)
            return RestResponse<Order>.NotFound($"User was not found");

        return stakeHolder.CanAccessOrder(order);
    }

    public async Task<RestResponse<bool>> UpdateStatusAsync(Guid requesterUserId, Guid orderId, UpdateOrderStatusRequest request)
    {
        var orderResult = await GetByUserForReadAsync(requesterUserId, orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return orderResult.Value.TryUpdateTo(request.To, request.Payload);
    }

    public async Task<RestResponse<bool>> CancelAsync(Guid requesterUserId, Guid orderId)
    {
        var order = await _orders.GetInstanceAsync(orderId);
        if (order is null)
            return RestResponse<bool>.NotFound($"Order ({orderId}) was not found");

        var stakeHolder = await _stakeHolders.GetInstanceAsync(requesterUserId);
        if (stakeHolder is null)
            return RestResponse<bool>.NotFound($"User was not found");

        var canUserCancelOrder = stakeHolder.CanCancelOrder(order);
        if (!canUserCancelOrder.IsSuccess)
            return canUserCancelOrder.MapTo(false);

        return TryCancelOrder(order);
    }

    private RestResponse<bool> TryCancelOrder(Order order)
    {
        if (!order.Status.CanBeCancelled)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot be cancelled!");

        order.Cancel();
        return RestResponse<bool>.Success(true);
    }
}