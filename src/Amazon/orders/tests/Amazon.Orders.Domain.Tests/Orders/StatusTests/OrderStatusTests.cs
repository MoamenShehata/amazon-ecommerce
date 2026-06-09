using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Orders.ValueObjects.Status;
using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.Customers;

namespace Amazon.Orders.Domain.Tests.Orders.StatusTests;

public class OrderStatusTests
{
    private readonly OrderFactory _orderFactory;
    public OrderStatusTests()
    {
        _orderFactory = new OrderFactory(RepoFactory.Create<Product, Guid>());
    }
    private async Task<Order> PlaceNewOrderAsync() => await _orderFactory.CreateAsync(Guid.NewGuid(), new CustomerInfo(Guid.NewGuid(), "mo@mo.com", "+20202"), new List<KeyValuePair<Guid, int>>(), null);

    [Fact]
    public async Task New_Order_CannotBeCancelled_UsingNormaStatus_Flow()
    {
        var order = await PlaceNewOrderAsync();

        Assert.Throws<InvalidOperationException>(() => order.TryUpdateTo(Domain.Orders.ValueObjects.Status.OrderState.Cancelled, null));
    }

    [Theory]
    [InlineData(OrderState.Pending)]
    [InlineData(OrderState.Processing)]
    [InlineData(OrderState.Shipped)]
    [InlineData(OrderState.DeliveryRecieved)]
    [InlineData(OrderState.CustomerDelivered)]
    public async Task Being_Processed_Order_CannotBeUpdated_To_Anything_Except_ShippingStarted(OrderState expectedToBeNextWrongState)
    {
        var order = await PlaceNewOrderAsync();
        order.TryUpdateTo(OrderState.Processing, null);
        Assert.Equal(OrderState.Processing, order.Status.State);

        var result = order.TryUpdateTo(expectedToBeNextWrongState, null);
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData(OrderState.Pending)]
    [InlineData(OrderState.Processing)]
    [InlineData(OrderState.ShippingStarted)]
    [InlineData(OrderState.DeliveryRecieved)]
    [InlineData(OrderState.CustomerDelivered)]
    public async Task ShippingStarted_Order_CannotBeUpdated_To_Anything_Except_Shipped(OrderState expectedToBeNextWrongState)
    {
        var order = await PlaceNewOrderAsync();
        order.TryUpdateTo(OrderState.Processing, null);

        order.TryUpdateTo(OrderState.ShippingStarted, new ShippingCompanyInfo("asd", "asd", "asd", "asdads"));
        Assert.Equal(OrderState.ShippingStarted, order.Status.State);

        var result = order.TryUpdateTo(expectedToBeNextWrongState, "");
        Assert.False(result.IsSuccess);
    }


    [Theory]
    [InlineData(OrderState.Pending)]
    [InlineData(OrderState.Processing)]
    [InlineData(OrderState.ShippingStarted)]
    [InlineData(OrderState.Shipped)]
    [InlineData(OrderState.CustomerDelivered)]
    public async Task Shipped_Order_CannotBeUpdated_To_Anything_Except_DeliveryRecieved(OrderState expectedToBeNextWrongState)
    {
        var order = await PlaceNewOrderAsync();
        order.TryUpdateTo(OrderState.Processing, null);

        order.TryUpdateTo(OrderState.ShippingStarted, new ShippingCompanyInfo("asd", "asd", "asd", "asdads"));

        order.TryUpdateTo(OrderState.Shipped, "trackingId");
        Assert.Equal(OrderState.Shipped, order.Status.State);

        var result = order.TryUpdateTo(expectedToBeNextWrongState, "");
        Assert.False(result.IsSuccess);
    }


    [Theory]
    [InlineData(OrderState.Pending)]
    [InlineData(OrderState.Processing)]
    [InlineData(OrderState.ShippingStarted)]
    [InlineData(OrderState.Shipped)]
    [InlineData(OrderState.DeliveryRecieved)]
    public async Task DeliveredToDeliveryGuy_Order_CannotBeUpdated_To_Anything_Except_CustomerDelivered(OrderState expectedToBeNextWrongState)
    {
        var order = await PlaceNewOrderAsync();
        order.TryUpdateTo(OrderState.Processing, null);

        order.TryUpdateTo(OrderState.ShippingStarted, new ShippingCompanyInfo("asd", "asd", "asd", "asdads"));

        order.TryUpdateTo(OrderState.Shipped, "trackingId");

        order.TryUpdateTo(OrderState.DeliveryRecieved, new DeliveryMember("asd", "asd"));
        Assert.Equal(OrderState.DeliveryRecieved, order.Status.State);

        var result = order.TryUpdateTo(expectedToBeNextWrongState, "");
        Assert.False(result.IsSuccess);
    }

    [Theory]
    [InlineData(OrderState.Pending)]
    [InlineData(OrderState.Processing)]
    [InlineData(OrderState.ShippingStarted)]
    [InlineData(OrderState.Shipped)]
    [InlineData(OrderState.DeliveryRecieved)]
    [InlineData(OrderState.CustomerDelivered)]
    public async Task CustomerDelivered_Order_CannotBeUpdated_To_Anything(OrderState expectedToBeNextWrongState)
    {
        var order = await PlaceNewOrderAsync();
        order.TryUpdateTo(OrderState.Processing, null);

        order.TryUpdateTo(OrderState.ShippingStarted, new ShippingCompanyInfo("asd", "asd", "asd", "asdads"));

        order.TryUpdateTo(OrderState.Shipped, "trackingId");

        order.TryUpdateTo(OrderState.DeliveryRecieved, new DeliveryMember("asd", "asd"));

        order.TryUpdateTo(OrderState.CustomerDelivered, null);
        Assert.Equal(OrderState.CustomerDelivered, order.Status.State);

        var result = order.TryUpdateTo(expectedToBeNextWrongState, "");
        Assert.False(result.IsSuccess);
    }


}