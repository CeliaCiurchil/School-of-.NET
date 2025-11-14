using Cafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Pricing
{
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
}
