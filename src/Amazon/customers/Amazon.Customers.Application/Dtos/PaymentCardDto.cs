namespace Amazon.Customers.Application.Dtos;

public class PaymentCardDto
{
    public int Id { get; set; }
    public string NumberMasked { get; set; }
    public string Expiration { get; set; }
}