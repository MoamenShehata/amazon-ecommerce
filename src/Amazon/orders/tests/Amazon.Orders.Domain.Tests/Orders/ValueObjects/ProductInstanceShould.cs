using Amazon.Orders.Domain.Orders.Factories;
using Amazon.Orders.Domain.Orders.ValueObjects;

namespace Amazon.Orders.Domain.Tests.Orders.ValueObjects;

public class ProductInstanceShould
{
    private ProductInstanceFactory _instanceFactory = new();

    [Theory]
    [InlineData("E5731E9D-7836-4D77-A159-E0097C7D93AF", 378)]
    [InlineData("E7159096-15C4-422E-9409-57D89F515C3D", 1205)]
    [InlineData("38A6AD6E-FB2D-43B0-9994-2724C4A91634", 1698.02)]
    public void Compare_Equality_By_Value(string pId, decimal unitPrice)
    {
        var id = Guid.Parse(pId);

        var p1 = new ProductInstance(id, unitPrice);
        var p2 = new ProductInstance(id, unitPrice);

        Assert.Equal(p1, p2);
    }

    private static List<ProductInstance> ProductInstancesPool = new List<ProductInstance>();

    [Fact]
    public void Create_OrderItems_Correctly()
    {
        var productsRepo = new List<KeyValuePair<Guid, decimal>>()
        {
            new KeyValuePair<Guid, decimal>(Guid.Parse("E5731E9D-7836-4D77-A159-E0097C7D93AF"),500),
            new KeyValuePair<Guid, decimal>(Guid.Parse("E7159096-15C4-422E-9409-57D89F515C3D"),400),
            new KeyValuePair<Guid, decimal>(Guid.Parse("38A6AD6E-FB2D-43B0-9994-2724C4A91634"),300),
        };

        var shoppingCart = new List<KeyValuePair<Guid, int>>(){
            new KeyValuePair<Guid, int>(Guid.Parse("E5731E9D-7836-4D77-A159-E0097C7D93AF"),2), //5
            new KeyValuePair<Guid, int>(Guid.Parse("E7159096-15C4-422E-9409-57D89F515C3D"),1), //4
            new KeyValuePair<Guid, int>(Guid.Parse("38A6AD6E-FB2D-43B0-9994-2724C4A91634"),10),
            new KeyValuePair<Guid, int>(Guid.Parse("E7159096-15C4-422E-9409-57D89F515C3D"),3), //4
            new KeyValuePair<Guid, int>(Guid.Parse("E5731E9D-7836-4D77-A159-E0097C7D93AF"),3), //5
        };

        var orderItems = shoppingCart.GroupBy(x => x.Key).Select(group => _instanceFactory.Create(group.Key, productsRepo.FirstOrDefault(x => x.Key == group.Key).Value).CreateOrderItem(group.Sum(z => z.Value)));

        var firstItem = orderItems.First();

        Assert.Equal(5, firstItem.Quantity);
        Assert.Equal(3, orderItems.Count());
    }
}