using Cafe.Domain.Events;
using System.Globalization;

namespace Cafe.Infrastructure.Observers;

public class ConsoleOrderLogger : IOrderEventSubscriber
{
    public void On(OrderPlaced evt)
    {
        Console.WriteLine($"""
                Order {evt.OrderId} {evt.PlacedAt}
                Items: {evt.Description}
                Subtotal: {evt.Subtotal.ToString("C", CultureInfo.CurrentCulture)}
                Pricing: {evt.PricingType}
                Total: {evt.Total.ToString("C", CultureInfo.CurrentCulture)}
            """);
    }
}
