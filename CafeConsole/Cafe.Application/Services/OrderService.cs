using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cafe.Domain.Beverages.Interfaces;
using Cafe.Domain.Enums;
using Cafe.Domain.Events;
using Cafe.Domain.Factories;
using Cafe.Domain.Beverages.Decorators;
using Cafe.Domain.Pricing;
using Cafe.Application.Models.Orders;

namespace Cafe.Application.Services;

public class OrderService : IOrderService
{
    private readonly IBeverageFactory beverageFactory;

    private readonly IOrderEventPublisher publisher;

    private readonly IPricingStrategyManager pricingStrategyManager;

    public OrderService(IBeverageFactory beverageFactory, IOrderEventPublisher publisher, IPricingStrategyManager pricingStrategyManager)
    {
        this.beverageFactory = beverageFactory ?? throw new ArgumentNullException(nameof(beverageFactory));
        this.publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        this.pricingStrategyManager = pricingStrategyManager ?? throw new ArgumentNullException(nameof(pricingStrategyManager));
    }

    public IBeverage ChooseBase(BeverageType beverageType)
    {
        return beverageFactory.CreateBeverage(beverageType);
    }

    private static IBeverage ApplyAddOns(IBeverage beverage, IEnumerable<AddOnChoice> addOns)
    {
        foreach (var addOn in addOns)
        {
            switch (addOn.Name)
            {
                case AddOn.Milk:
                    beverage = new MilkDecorator(beverage);
                    break;

                case AddOn.ExtraShot:
                    beverage = new ExtraShotDecorator(beverage);
                    break;

                case AddOn.Syrup:
                    var flavor = string.IsNullOrWhiteSpace(addOn.Option) ? "vanilla" : addOn.Option;
                    beverage = new SyrupDecorator(beverage, flavor);
                    break;

                default:
                    Console.WriteLine($"Unknown add-on '{addOn.Name}', ignored.");
                    break;
            }
        }
        return beverage;
    }


    public OrderPlaced PlaceOrder(OrderRequest orderRequest)
    {
        var beverage = ChooseBase(orderRequest.BaseDrink);

        beverage = ApplyAddOns(beverage, orderRequest.AddOns);

        var subtotal = beverage.Cost();
        var strategy = pricingStrategyManager.GetStrategy(orderRequest.PricingPolicy);
        var total = strategy.Apply(subtotal);

        var order = new OrderPlaced
        {
            OrderId = Guid.NewGuid(),
            PlacedAt = DateTimeOffset.UtcNow,
            Description = beverage.Describe(),
            Subtotal = subtotal,
            Total = total,
            PricingType = orderRequest.PricingPolicy
        };

        publisher.Publish(order);
        return order;
    }
}
