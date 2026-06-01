namespace Amazon.Cart.Application.Dtos;

public class UpdateCartDto
{
    public int DeliverToAddressId { get; set; }
    public Guid PaymentMethodId { get; set; }
}
