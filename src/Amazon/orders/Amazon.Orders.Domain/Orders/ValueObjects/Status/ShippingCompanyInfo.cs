using System.ComponentModel.DataAnnotations;

namespace Amazon.Orders.Domain.Orders.ValueObjects.Status;

public record ShippingCompanyInfo(string Address, string PhoneNumber, string Name, string Website);