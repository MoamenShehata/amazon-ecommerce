using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
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

    public async Task<RestResponse<bool>> StartProcessingAsync(Guid orderId)
    {
        var orderResult = await GetByIdAsync(orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return TryStartProcessingAsync(orderResult);
    }

    public async Task<RestResponse<bool>> StartShippingAsync(Guid orderId)
    {
        var orderResult = await GetByIdAsync(orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return TryStartShippingAsync(orderResult);
    }

    public async Task<RestResponse<bool>> ShippingCompletedAsync(Guid orderId)
    {
        var orderResult = await GetByIdAsync(orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return TryCompleteShippingAsync(orderResult);
    }

    public async Task<RestResponse<bool>> DeliveryAcceptedAsync(Guid orderId)
    {
        var orderResult = await GetByIdAsync(orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return TryDeliveryAcceptedAsync(orderResult);
    }

    public async Task<RestResponse<bool>> CompletedAsync(Guid orderId)
    {
        var orderResult = await GetByIdAsync(orderId);
        if (!orderResult.IsSuccess)
            return orderResult.MapTo(false);

        return TryCompleteAsync(orderResult);
    }

    private RestResponse<bool> TryCancelOrder(Order order)
    {
        if (!order.Status.CanBeCancelled)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot be cancelled!");

        order.Cancel();
        return RestResponse<bool>.Success(true);
    }

    private RestResponse<bool> TryStartProcessingAsync(Order order)
    {
        if (order.Status.State != OrderState.Created)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot be started to process!");

        order.UpdateStatus(new OrderProcessingStatus(order.Id));
        return RestResponse<bool>.Success(true);
    }

    private RestResponse<bool> TryStartShippingAsync(Order order)
    {
        if (order.Status.State != OrderState.Processing)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot be started to shipp!");

        order.StartShipping("95874ab-87", new ShippingCompanyInfo("Egypt, Sharqia, 75, 10th of Ramdan Ordnia road", " + 201645454", "Bosta", "https//www.google.com"));
        return RestResponse<bool>.Success(true);
    }

    private RestResponse<bool> TryCompleteShippingAsync(Order order)
    {
        if (order.Status.State != OrderState.ShippingStarted)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot be updated to ship completed!");

        order.UpdateStatus(new OrderShippedStatus(order.Id));
        return RestResponse<bool>.Success(true);
    }

    private RestResponse<bool> TryDeliveryAcceptedAsync(Order order)
    {
        if (order.Status.State != OrderState.Shipped)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot be updated to delivery accepted!");

        order.DeliveryAccepted(new DeliveryMember("Mohsen abady", "01127970304"));
        return RestResponse<bool>.Success(true);
    }

    private RestResponse<bool> TryCompleteAsync(Order order)
    {
        if (order.Status.State != OrderState.DeliveryRecieved)
            return RestResponse<bool>.BadRequest($"Order of id {order.Id} cannot close and complete the order!");

        order.Complete();
        return RestResponse<bool>.Success(true);
    }
}