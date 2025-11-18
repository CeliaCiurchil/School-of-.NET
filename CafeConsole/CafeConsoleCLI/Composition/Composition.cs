using Cafe.Application;
using Cafe.Application.Services;
using Cafe.CLI.Menus;
using Cafe.Domain.Events;
using Cafe.Domain.Factories;
using Cafe.Domain.Pricing;
using Cafe.Infrastructure.Factories;
using Cafe.Infrastructure.Observers;
using Microsoft.Extensions.DependencyInjection;

namespace CafeConsoleCLI.Composition;

public static class Composition
{
    public static IServiceProvider Build()
    {
        var services = new ServiceCollection();

        services.AddSingleton<IBeverageFactory, BeverageFactory>();

        services.AddSingleton<RegularPricing>();
        services.AddSingleton<HappyHourPricing>();
        services.AddSingleton<IPricingStrategyManager, PricingManager>();

        services.AddSingleton<IOrderEventPublisher, SimpleOrderEventPublisher>();
        services.AddSingleton<IOrderEventSubscriber, ConsoleOrderLogger>();
        services.AddSingleton<IOrderEventSubscriber, InMemoryOrderAnalytics>();

        services.AddSingleton<IOrderService, OrderService>();

        services.AddSingleton<OrderMenu>();
        services.AddSingleton<Menu>();

        var sp = services.BuildServiceProvider();
        WireEvents(sp);
        return sp;
    }

    private static void WireEvents(IServiceProvider sp)
    {
        var publisher = sp.GetRequiredService<IOrderEventPublisher>();
        foreach (var sub in sp.GetServices<IOrderEventSubscriber>())
            publisher.Subscribe(sub);
    }
}