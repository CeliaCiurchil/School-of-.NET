using Cafe.Domain.Enums;

namespace Cafe.Domain.Pricing;

public interface IPricingStrategyManager
{
    public IPricingStrategy GetStrategy(PricingType pricingType);
}
