using Amazon.SharedKernel.Orders.Commands;
using Amazon.Shipping.Domain;
using Amazon.Shipping.Domain.Companies;
using Amazon.Shipping.Domain.Strategies;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;

namespace Amazon.Shipping.Application;

public class ShippingAppService(
    IRepository<ShipmentRequest, Guid> _shipmentRequests,
    ShippingCompanyStrategy _companyStrategy,
    IUnitOfWork _unitOfWork)
{
    public async Task CreateShipmentRequestAsync(CreateShipmentCommand command)
    {
        var request = new ShipmentRequest(command.OrderId, command.Customer, command.DeliverToAddress);
        _shipmentRequests.Add(request);

        await _unitOfWork.CommitAsync();
    }

    public async Task<ShippingCompany> AssignShipmentToCompanyAsync(Guid shipmentRequestId)
    {
        var request = await _shipmentRequests.GetInstanceAsync(shipmentRequestId);

        var companyToShipOrder = await _companyStrategy.SelectForRequestAsync(request);

        request.AssignedToCompany(companyToShipOrder);
        await _unitOfWork.CommitAsync();

        return companyToShipOrder;
    }
}