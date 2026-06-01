namespace Amazon.Customers.Application.Dtos;

public record PaymentCardDto(int Id, string CardHolder, string CardNumber, string ExpiresAt);