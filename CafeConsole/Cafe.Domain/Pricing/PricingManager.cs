using Cafe.Domain.Enums;

namespace Cafe.Domain.Pricing;

public class PricingManager : IPricingStrategyManager
{
    private readonly RegularPricing regularPricing;
    private readonly HappyHourPricing happyHourPricing;

    public PricingManager(RegularPricing regularPricing, HappyHourPricing happyHourPricing)
    {
        this.regularPricing = regularPricing;
        this.happyHourPricing = happyHourPricing;
    }

    public IPricingStrategy GetStrategy(PricingType pricingType) => pricingType switch
    {
        PricingType.Regular => regularPricing,
        PricingType.Discount => happyHourPricing,
        _ => throw new ArgumentOutOfRangeException(nameof(pricingType), pricingType, null)
    };
}
