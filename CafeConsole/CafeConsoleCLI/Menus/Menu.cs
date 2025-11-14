using Cafe.Application.Services;
using Cafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cafe.Infrastructure.Factories;
namespace Cafe.CLI.Menus;

static class Menu
{
    public static void ShowMainMenu()
    {
        Console.Clear();
        Console.WriteLine("Welcome to Cafe Console CLI");
    }
    public static void ShowBeverageMenu()
    {
        Console.WriteLine("Choose a base beverage:");
        foreach (var beverage in Enum.GetValues<BeverageType>())
        {
            Console.WriteLine($"- {beverage}");
        }
        Console.Write("Enter beverage name: ");
    }
    public static void Run()
    {
        int option = 1;
        while (option != 0)
        {
            try
            {
                ShowMainMenu();
                ShowBeverageMenu();
                ChooseBaseBeverages();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Press any key to continue");
                Console.ReadKey(true);
                Console.Clear();
            }
        }
    }

    private static void ChooseBaseBeverages()
    {
        BeverageType beverageType;

        if (!Enum.TryParse(Console.ReadLine(),true, out beverageType))
        {
            Console.WriteLine("Invalid input. Please enter a beverage type from the list.");
        }
        OrderService orderService = new(new BeverageFactory());
        orderService.ChooseBase(beverageType);
    }
}
