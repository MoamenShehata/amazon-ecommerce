using Amazon.Customers.Application.CustomerProfiles;
using Amazon.Customers.Application.CustomerProfiles.Models;
using Amazon.Customers.Application.Dtos;
using Amazon.Customers.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Amazon.Customers.Infrastructure.Services;

public class CustomerProfileService(CustomerReadContext _readContext) : ICustomerProfileService
{
    public async Task<CustomerProfile> GetByIdAsync(Guid customerId)
    {
        return await _readContext.CustomerProfiles.AsNoTracking().FirstOrDefaultAsync(x => x.CustomerId == customerId);
    }

    public async Task CreateAsync(CustomerProfile customerProfile)
    {
        _readContext.CustomerProfiles.Add(customerProfile);
        await _readContext.SaveChangesAsync();
    }

    public async Task UpdateShippingAddressesAsync(Guid customerId, ICollection<CustomerProfileAddress> newAddresses)
    {
        var profile = await _readContext.CustomerProfiles.FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (profile is null) return;

        profile.Addresses = newAddresses;
        await _readContext.SaveChangesAsync();
    }

    public async Task UpdatePaymentCardsAsync(Guid customerId, ICollection<PaymentCardDto> newCards)
    {
        var profile = await _readContext.CustomerProfiles.FirstOrDefaultAsync(x => x.CustomerId == customerId);
        if (profile is null) return;

        profile.PaymentCards = newCards;
        await _readContext.SaveChangesAsync();
    }
}
