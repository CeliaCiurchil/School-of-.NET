using Cafe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Infrastructure.Observers;

public class InMemoryOrderAnalytics : IOrderEventSubscriber
{
    private int totalOrders = 0;
    private decimal totalRevenue = 0m;

    public void On(OrderPlaced evt)
    {
        totalOrders++;
        totalRevenue += evt.Total;

        Console.WriteLine($"[Analytics] Running totals Orders: {totalOrders}, Revenue: {totalRevenue:C}");
    }

    public int TotalOrders => totalOrders;

    public decimal TotalRevenue => totalRevenue;
}
