using Amazon.Cart.Domain.ShoppingCarts;
using Stripe.Checkout;

namespace Amazon.Cart.Application.Mappers;

internal static class CartToStripeMapper
{
    internal static List<SessionLineItemOptions> ToSessionLineItems(this ShoppingCart cart)
    {
        return cart.Items.Select(c => new SessionLineItemOptions
        {
            Quantity = c.Quantity,
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "egp",
                UnitAmountDecimal = c.TotalPrice * 100,
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = c.Info.Name,
                }
            }
        }).ToList();
    }
}