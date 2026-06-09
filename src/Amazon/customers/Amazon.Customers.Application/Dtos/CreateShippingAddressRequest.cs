using Amazon.SharedKernel.Customers;

namespace Amazon.Customers.Application.Dtos;

public class CreateShippingAddressRequest
{
    public CityInfo City { get; set; }
    public HouseInfo House { get; set; }
    public bool IsDefault { get; set; }
}