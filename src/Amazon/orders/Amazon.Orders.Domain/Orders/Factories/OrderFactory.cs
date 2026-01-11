using Amazon.Orders.Domain.Orders.Events;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
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

            //Func<Product, ProductInstance> productInstanceCacheFactory = p => _productInstanceFactory.CreateAsync(p.Id, p.CurrentPrice);
            //var orderItems = products.Select(p => productInstanceCacheFactory(p).CreateOrderItem(cartItems.Where(x => x.Key == p.Id).Sum(x => x.Value))).ToList();

            Func<Product, OrderItem> orderItemFactory = p => p.CreateOrderItem(cartItems.Where(x => x.Key == p.Id).Sum(x => x.Value));

            var order = new Order(customerInfo, products.Select(orderItemFactory).ToList());

            order.RaiseEvent(new OrderCreatedEvent(DateTime.UtcNow, order.Id));

            return order;
        }
    }
}