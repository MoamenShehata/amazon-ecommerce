namespace Amazon.Customers.Application.Dtos;

public record PaymentCardForIntegrationDto(int Id, string CardHolder, string OriginalNumber, string MaskedNumber, int ExpiryMonth, int ExpiryYear);
