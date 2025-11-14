using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Pricing
{
    internal class HappyHourPricing : IPricingStrategy
    {
        private const decimal DiscountRate = 0.20m;
        public decimal Apply(decimal subtotal)
        {
            if(subtotal<0)
            {
                throw new ArgumentOutOfRangeException(nameof(subtotal), "Subtotal cannot be negative.");
            }

            decimal discount = DiscountRate * subtotal;
            decimal total = subtotal - discount;

            return total;
        }
    }
}
