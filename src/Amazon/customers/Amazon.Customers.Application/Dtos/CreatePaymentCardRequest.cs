namespace Amazon.Customers.Application.Dtos;

public record CreatePaymentCardRequest(string CardHolder, string CardNumber, DateTime ExpiresAt);