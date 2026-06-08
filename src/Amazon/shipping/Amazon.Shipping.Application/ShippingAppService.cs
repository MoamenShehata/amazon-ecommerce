using Amazon.SharedKernel.Customers;
using Amazon.SharedKernel.Orders.Commands;
using Amazon.Shipping.Domain;
using Amazon.Shipping.Domain.Companies;
using Amazon.Shipping.Domain.ValueObjects;
using Moamen.SDKs.Repository;
using Moamen.SDKs.SharedKernel;
using System.Text.Json;

namespace Amazon.Shipping.Application
{
    public class ShippingAppService(
        IRepository<ShipmentRequest, Guid> _shipmentRequests,
        IUnitOfWork _unitOfWork)
    {
        public async Task CreateShipmentRequestAsync(CreateShipmentCommand command)
        {
            var address = JsonSerializer.Deserialize<SharedKernel.Customers.DeliveryAddress>(command.DeliverToAddressJson);

            var request = new ShipmentRequest(command.OrderId, new CustomerInfo(command.CustomerId, command.CustomerEmail, command.CustomerPhone), new Domain.ValueObjects.DeliveryAddress(new AddressCity(address.CountryId, address.CityId, address.PostalCode), new AddressAppartment(address.BuildingNumber, address.ApartmentNumber)));
            _shipmentRequests.Add(request);

            await _unitOfWork.CommitAsync();
        }

        public async Task<ShippingCompany> AssignShipmentToCompanyAsync(Guid shipmentRequestId)
        {
            var request = await _shipmentRequests.GetInstanceAsync(shipmentRequestId);

            // pick one existing company to ship the request to the customer
        }
    }
}