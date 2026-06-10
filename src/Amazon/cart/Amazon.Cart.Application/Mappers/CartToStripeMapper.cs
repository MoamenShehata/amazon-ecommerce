using Amazon.Cart.Domain;
using Stripe.Checkout;

namespace Amazon.Cart.Application.Mappers;

internal static class CartToStripeMapper
{
    internal static List<SessionLineItemOptions> ToSessionLineItems(this ShoppingCart cart)
    {
        var products = cart.Items.DistinctBy(x => x.ProductId).Select(x => x.Product);

        return cart.AggregatToProducts.Select(p => new SessionLineItemOptions
        {
            Quantity = p.Value,
            PriceData = new SessionLineItemPriceDataOptions
            {
                Currency = "egp",
                UnitAmountDecimal = products.FirstOrDefault(x => x.Id == p.Key).Info.UnitPrice * 100,
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = products.FirstOrDefault(x => x.Id == p.Key).Info.Name,
                }
            }
        }).ToList();
    }
}