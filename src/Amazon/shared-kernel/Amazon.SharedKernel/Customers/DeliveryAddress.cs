namespace Amazon.SharedKernel.Customers;

public class DeliveryAddress
{
    public CityInfo City { get; private set; }
    public HouseInfo Appartment { get; private set; }

    public DeliveryAddress(CityInfo city, HouseInfo appartment)
    {
        City = city;
        Appartment = appartment;
    }

    #region MyRegion
    private DeliveryAddress()
    {

    }
    #endregion
}