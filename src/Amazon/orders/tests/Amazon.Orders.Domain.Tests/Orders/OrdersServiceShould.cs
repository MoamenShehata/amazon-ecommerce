using System.Net;
using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;

namespace Amazon.Orders.Domain.Tests.Orders
{
    public class OrdersServiceShould
    {
        private OrdersService CreateOrderService()
        {
            var productRepo = RepoFactory.Create<Product, Guid>();

            var productService = new ProductsService(productRepo);
            var orderFactory = new OrderFactory(productRepo);


            return new OrdersService(
                RepoFactory.Create<Order, Guid>(),
                productService,
                orderFactory
                );
        }

        [Fact]
        public async Task Return_ServerFailure_When_PlaceOrder()
        {
            var orderService = CreateOrderService();

            var cartItems = new List<KeyValuePair<Guid, int>>
            {
                new KeyValuePair<Guid, int>(Guid.NewGuid(),10)
            };

            var result = await orderService.PlaceOrderAsync(new CustomerInfo(Guid.NewGuid(), "mo@mo.com"), cartItems);

            Assert.False(result.IsSuccess);
            Assert.Null(result.Value);
            Assert.Equal(HttpStatusCode.InternalServerError, result.StatusCode);
        }


        [Fact]
        public async Task PlaceOrder_Correctly()
        {
            var orderService = CreateOrderService();

            var cartItems = new List<KeyValuePair<Guid, int>>
            {
                new KeyValuePair<Guid, int>()
            };

            var result = await orderService.PlaceOrderAsync(new CustomerInfo(Guid.NewGuid(), "mo@mo.com"), cartItems);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }
    }
}