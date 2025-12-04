namespace Cafe.CLI.Menus;

public sealed class Menu
{
    private readonly OrderMenu orderMenu;
    public Menu(OrderMenu orderMenu) => this.orderMenu = orderMenu;

    public void Run()
    {
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Welcome to Cafe\n");

            orderMenu.PlaceOrder();

            Console.WriteLine("\nPress 1 to EXIT, or any other key to place another order...");

            var input = Console.ReadLine();
            if (!string.IsNullOrEmpty(input) && input.Trim().Equals("1")) return;
        }
    }
}
