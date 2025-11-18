using Cafe.Domain.Enums;

namespace Cafe.Domain.Pricing;

public class RegularPricing : IPricingStrategy
{
    public PricingType pricingType { get; } = PricingType.Regular;

    public decimal Apply(decimal subtotal) => subtotal;
}
