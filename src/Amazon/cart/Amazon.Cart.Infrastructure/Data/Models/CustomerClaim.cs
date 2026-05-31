namespace Amazon.Cart.Infrastructure.Data.Models
{
    public class CustomerClaim
    {
        public Guid CustomerId { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}