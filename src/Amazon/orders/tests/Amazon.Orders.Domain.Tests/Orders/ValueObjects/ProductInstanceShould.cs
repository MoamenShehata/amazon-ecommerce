using Amazon.Orders.Domain.Orders.ValueObjects;

namespace Amazon.Orders.Domain.Tests.Orders.ValueObjects;

public class ProductInstanceShould
{
    [Theory]
    [InlineData("E5731E9D-7836-4D77-A159-E0097C7D93AF", 378,"p1")]
    [InlineData("E7159096-15C4-422E-9409-57D89F515C3D", 1205, "p2")]
    [InlineData("38A6AD6E-FB2D-43B0-9994-2724C4A91634", 1698.02, "p3")]
    public void Compare_Equality_By_Value(string pId, decimal unitPrice, string name)
    {
        var id = Guid.Parse(pId);

        var p1 = new ProductInfo(id, unitPrice, name);
        var p2 = new ProductInfo(id, unitPrice, name);

        Assert.Equal(p1, p2);
    }
}