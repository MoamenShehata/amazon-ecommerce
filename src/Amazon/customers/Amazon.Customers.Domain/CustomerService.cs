using Amazon.Customers.Domain.Entities;
using Amazon.Customers.Domain.Events;
using Amazon.Customers.Domain.ValueObjects;
using Amazon.SharedKernel.API;
using Amazon.SharedKernel.Extensions;
using Moamen.SDKs.Repository;
using static Amazon.Customers.Domain.ValueObjects.PaymentCardNumber;

namespace Amazon.Customers.Domain;

public class CustomerService(
    IRepository<Customer, Guid> _repository,
    IAddressService _addressService,
    PaymentCardNumberFactory _cardNumberFactory
    )
{
    public async Task<RestResponse<Customer>> GetByIdAsync(Guid customerId)
    {
        var customer = await _repository.GetInstanceAsync(customerId);
        if (customer is null)
            return RestResponse<Customer>.NotFound($"Customer with id {customerId} not found.");

        return RestResponse<Customer>.Success(customer);
    }

    public async Task<RestResponse<Customer>> CreateCustomerAsync(Guid id, string email, string phoneNumber)
    {
        var customerResult = await GetByIdAsync(id);
        if (customerResult.IsSuccess)
            return customerResult;

        var newCustomer = new Customer(id, new ContactInfo(email, phoneNumber));
        newCustomer.RaiseEvent(new CustomerCreatedEvent(newCustomer.Id));

        _repository.Add(newCustomer);
        return RestResponse<Customer>.Created(newCustomer, newCustomer.Id.ToString());
    }

    public async Task<RestResponse<PaymentCard>> CreatePaymentCardAsync(Guid customerId, string holderName, string number, DateTime expiresAt)
    {
        var customerResult = await GetByIdAsync(customerId);
        if (!customerResult.IsSuccess)
            return customerResult.MapTo(null as PaymentCard);

        var cardNumber = await _cardNumberFactory.CreateSecuredAsync(number);

        return customerResult.Value.AddPaymentCard(new PaymentCardInfo(holderName, cardNumber, new PaymentCardExpiration(expiresAt)));
    }

    public async Task<RestResponse<bool>> AddShippingAddressAsync(Guid customerId, CityInfo city, HouseInfo house, bool isDefault)
    {
        var customerResult = await GetByIdAsync(customerId);
        if (!customerResult.IsSuccess)
            return customerResult.MapTo(false);

        var cityLookupResult = await _addressService.GetCityInfoAsync(city.CountryId, city.CityId);
        if (!cityLookupResult.IsSuccess)
            return cityLookupResult.MapTo(false);

        var addResult = customerResult.Value.AddShippingAddress(new Entities.ShippingAddress(customerId, city, house, isDefault));
        if (!addResult.IsSuccess)
            return addResult.MapTo(false);

        return RestResponse<bool>.Success(true);
    }

    public async Task<RestResponse<PaymentCard>> GetPaymentCardAsync(Guid customerId, int cardId)
    {
        var customerResult = await GetByIdAsync(customerId);
        if (!customerResult.IsSuccess)
            return customerResult.MapTo(null as PaymentCard);

        var card = customerResult.Value.PaymentCards.FirstOrDefault(c => c.Id == cardId);
        if (card is null)
            return RestResponse<PaymentCard>.NotFound($"Payment card with id {cardId} not found.");

        return RestResponse<PaymentCard>.Success(card);
    }
}