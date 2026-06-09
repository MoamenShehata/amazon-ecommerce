using Amazon.Orders.Domain.Orders;
using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;
using Amazon.Orders.Domain.Products;
using Amazon.Orders.Domain.Stakeholders;
using Amazon.SharedKernel.Customers;
using System.Net;

namespace Amazon.Orders.Domain.Tests.Orders
{
    public class OrdersServiceShould
    {
        private OrdersService CreateOrderService()
        {
            var productRepo = RepoFactory.Create<Product, Guid>();
            var productId = Guid.Parse("F954B880-3C4A-4AEA-AAFA-ADE614AE8576");
            productRepo.Add(new Product(productId, "product", 25, 500));


            var productService = new ProductsService(productRepo);
            var orderFactory = new OrderFactory(productRepo);


            return new OrdersService(
                RepoFactory.Create<Order, Guid>(),
                RepoFactory.Create<StakeHolder, Guid>(),
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

            var result = await orderService.PlaceOrderAsync(Guid.NewGuid(), new CustomerInfo(Guid.NewGuid(), "mo@mo.com","+20202"), cartItems, null, null);

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
                new KeyValuePair<Guid, int>(Guid.Parse("F954B880-3C4A-4AEA-AAFA-ADE614AE8576"),7)
            };

            var result = await orderService.PlaceOrderAsync(Guid.NewGuid(), new CustomerInfo(Guid.NewGuid(), "mo@mo.com", "+20202"), cartItems, null, null);

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Single(result.Value.Items);
            Assert.Equal(7, result.Value.Items.FirstOrDefault().Quantity);
            Assert.Equal(7 * 500, result.Value.Price);
            Assert.Equal(HttpStatusCode.Created, result.StatusCode);
        }
    }
}