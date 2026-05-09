using Moamen.SDKs.SharedKernel.DDD.Events;

namespace Amazon.SharedKernel.Customers.Events;

public record NewCustomerRegistrationEvent(Guid Id, string Email, string PhoneNumber) : IntegrationEvent(DateTime.UtcNow);