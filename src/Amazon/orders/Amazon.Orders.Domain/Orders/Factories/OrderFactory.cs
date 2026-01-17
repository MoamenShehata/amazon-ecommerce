using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Amazon.SharedKernel.Orders.Events;
using Moamen.SDKs.Repository;

namespace Amazon.Orders.Domain.Orders.Factories
{
    public class OrderFactory(
        IRepository<Product, Guid> _productsRepo
        )
    {
        public async Task<Order> CreateAsync(CustomerInfo customerInfo, List<KeyValuePair<Guid, int>> cartItems)
        {
            var products = await _productsRepo.GetAllAsync(p => cartItems.Select(x => x.Key).Distinct().Contains(p.Id));

            var orderId = Guid.NewGuid();

            Func<Product, OrderItem> orderItemFactory = p => p.CreateOrderItem(orderId, cartItems.Where(x => x.Key == p.Id).Sum(x => x.Value));

            var orderItems = products.Select(orderItemFactory).ToList();
            var order = new Order(orderId, customerInfo, orderItems);

            order.RaiseEvent(new OrderCreatedEvent(order.Id, orderItems.Select(i => new KeyValuePair<Guid, int>(i.ProductInfo.ProductId, i.Quantity)).ToList()));

            return order;
        }
    }
}