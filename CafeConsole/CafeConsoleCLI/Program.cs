using Cafe.Application;
using Cafe.Application.Services;
using Cafe.CLI.Menus;
using Cafe.Domain.Events;
using Cafe.Domain.Factories;
using Cafe.Domain.Pricing;
using Cafe.Infrastructure.Factories;
using Cafe.Infrastructure.Observers;
using Microsoft.Extensions.DependencyInjection;

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

var publisher = sp.GetRequiredService<IOrderEventPublisher>();
foreach (var sub in sp.GetServices<IOrderEventSubscriber>())
    publisher.Subscribe(sub);

sp.GetRequiredService<Menu>().Run();