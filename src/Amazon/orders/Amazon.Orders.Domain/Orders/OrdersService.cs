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
    public async Task<RestResponse<bool>> PlaceOrderAsync(CustomerInfo customerInfo, List<KeyValuePair<Guid, int>> cartItems)
    {
        var isInventoryInvalidForAnyProduct = await _productsService.IsAnyProductNotInventoryAvailableAsync(cartItems.Select(x => x.Key).ToList());
        if (isInventoryInvalidForAnyProduct)
            return RestResponse<bool>.Failure($"Cannot full fill this whole order request due to lack of some products in inventory");

        var order = await _orderFactory.CreateAsync(customerInfo, cartItems);
        _ordersRepo.Add(order);

        return RestResponse<bool>.Created(true, order.Id.ToString());
    }
}