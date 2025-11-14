using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cafe.Domain.Events;
namespace Cafe.Infrastructure.Observers;

public class ConsoleOrderLogger : IOrderEventSubscriber
{
    public void On(OrderPlaced evt)
    {
        Console.WriteLine($"""
                Order {evt.OrderId} {evt.PlacedAt}
                Items: {evt.Description}
                Subtotal: {evt.Subtotal.ToString("C",CultureInfo.CurrentCulture)}
                Pricing: {evt.PricingType}
                Total: {evt.Total.ToString("C",CultureInfo.CurrentCulture)}
            """);
    }
}
