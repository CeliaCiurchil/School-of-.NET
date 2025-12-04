using Cafe.Domain.Enums;

namespace Cafe.Domain.Pricing;

public class HappyHourPricing : IPricingStrategy
{
    private const decimal DiscountRate = 0.20m;

    public PricingType pricingType { get; } = PricingType.Discount;

    public decimal Apply(decimal subtotal)
    {
        if (subtotal < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(subtotal), "Subtotal cannot be negative.");
        }

        decimal discount = DiscountRate * subtotal;
        decimal total = subtotal - discount;

        return total;
    }
}
