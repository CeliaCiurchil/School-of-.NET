using Cafe.Domain.Pricing;
using Xunit;

namespace Cafe.Tests.Pricing
{
    public class HappyHourPricingTests
    {
        [Fact]
        public void Apply_WithTenDollars_ReturnsEightDollars()
        {
            IPricingStrategy pricing = new HappyHourPricing();
            decimal subtotal = 10.00m;

            decimal total = pricing.Apply(subtotal);

            Assert.Equal(8.00m, total);
        }

        [Fact]
        public void Apply_WithNegativeSubtotal_ThrowsArgumentOutOfRangeException()
        {
            IPricingStrategy pricing = new HappyHourPricing();
            decimal subtotal = -5.00m;

            Assert.Throws<ArgumentOutOfRangeException>(() => pricing.Apply(subtotal));
        }
    }
}
