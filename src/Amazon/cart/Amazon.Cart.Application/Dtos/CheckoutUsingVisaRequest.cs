namespace Amazon.Cart.Application.Dtos;

public class CheckoutUsingVisaRequest
{
    public int PaymentCardId { get; set; }
    public string Cvv { get; set; }
}