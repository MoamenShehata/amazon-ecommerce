namespace Amazon.Cart.Domain.Integrations.Customers.Dtos;

public record CustomerPaymentCard(string OriginalNumber, string MaskedNumber, int ExpireyMonth, int ExpiryYear);