namespace Cafe.Domain.Pricing;

public class RegularPricing : IPricingStrategy
{
    public decimal Apply(decimal subtotal) => subtotal;
}
