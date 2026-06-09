using Amazon.Shipping.Domain.Companies;
using Moamen.SDKs.Repository;

namespace Amazon.Shipping.Domain.Strategies;

public class ShippingCompanyStrategy(IRepository<ShippingCompany, Guid> _shippingComapnies)
{
    public async Task<ShippingCompany> SelectForRequestAsync(ShipmentRequest shipmentRequest)
    {
        // pick first active company, for simplicity
        return await _shippingComapnies.GetInstanceAsync(x => x.IsActive);
    }
}