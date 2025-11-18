using Cafe.Application.Models.Orders;
using Cafe.Application.Services;
using Cafe.Domain.Enums;

namespace Cafe.CLI.Menus;

public class OrderMenu
{
    private readonly IOrderService orderService;

    public OrderMenu(IOrderService orderService)
    {
        this.orderService = orderService ?? throw new ArgumentNullException(nameof(orderService));
    }

    public void PlaceOrder()
    {
        var baseDrink = AskBeverage();

        var addOns = AskAddOns();

        var pricing = AskPricing();

        var request = new OrderRequest(baseDrink, addOns, pricing);
        var evt = orderService.PlaceOrder(request);
    }

    private static BeverageType AskBeverage()
    {
        Console.WriteLine("\nChoose a base beverage:");
        foreach (var b in Enum.GetValues<BeverageType>())
            Console.WriteLine($"- {b}");

        while (true)
        {
            Console.Write("Enter beverage name: ");
            var input = Console.ReadLine();
            if (Enum.TryParse(input, true, out BeverageType result))
                return result;

            Console.WriteLine("Invalid beverage. Try again.");
        }
    }

    private static IReadOnlyList<AddOnChoice> AskAddOns()
    {
        var addOns = new List<AddOnChoice>();
        Console.WriteLine("\nAdd-ons:");
        Console.WriteLine("1) Milk  2) Syrup  3) Extra shot  0) Done");

        while (true)
        {
            Console.Write("Select: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    addOns.Add(new AddOnChoice(AddOn.Milk));
                    break;

                case "2":
                    Console.Write("Enter syrup flavor: ");
                    var flavor = Console.ReadLine();
                    addOns.Add(new AddOnChoice(AddOn.Syrup, flavor));
                    break;

                case "3":
                    addOns.Add(new AddOnChoice(AddOn.ExtraShot));
                    break;

                case "0":
                    return addOns;

                default: Console.WriteLine("Invalid option."); break;
            }
        }
    }

    private static PricingType AskPricing()
    {
        Console.WriteLine("\nPricing: 1) Regular  2) Happy Hour");
        while (true)
        {
            Console.Write("Select: ");
            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    return PricingType.Regular;

                case "2":
                    return PricingType.Discount;

                default:
                    Console.WriteLine("Invalid option.");
                    break;
            }
        }
    }
}
