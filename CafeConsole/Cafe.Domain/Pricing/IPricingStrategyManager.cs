using Cafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Pricing
{
    public interface IPricingStrategyManager
    {
        public IPricingStrategy GetStrategy(PricingType pricingType);
    }
}
