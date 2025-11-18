using Cafe.Domain.Enums;
using Cafe.Domain.Events;
using Cafe.Infrastructure.Observers;

namespace Cafe.Tests.Events;

public class InMemoryOrderAnalyticsTests
{
    [Fact]
    public void On_WhenTwoOrdersPublished_AccumulatesTotalsCorrectly()
    {
        var analytics = new InMemoryOrderAnalytics();
        var order1 = new OrderPlaced(Guid.NewGuid(), DateTimeOffset.Now, "Espresso", 3.50m, PricingType.Regular, 3.50m);
        var order2 = new OrderPlaced(Guid.NewGuid(), DateTimeOffset.Now, "Tea", 2.00m, PricingType.Regular, 2.00m);

        analytics.On(order1);
        analytics.On(order2);

        Assert.Equal(2, analytics.TotalOrders);
        Assert.Equal(5.50m, analytics.TotalRevenue);
    }
}
