using Cafe.Domain.Pricing;
using Xunit;

namespace Cafe.Tests.Pricing
{
    public class RegularPricingTests
    {
        [Fact]
        public void Apply_WithTenDollars_ReturnsSameAmount()
        {
            IPricingStrategy pricing = new RegularPricing();
            decimal subtotal = 10.00m;

            decimal total = pricing.Apply(subtotal);

            Assert.Equal(10.00m, total);
        }
    }
}
