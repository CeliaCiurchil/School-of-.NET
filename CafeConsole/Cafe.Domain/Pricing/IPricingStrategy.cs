using Cafe.Domain.Enums;

namespace Cafe.Domain.Pricing;

public interface IPricingStrategy
{
    public PricingType pricingType { get; }
    decimal Apply(decimal subtotal);
}
