using Cafe.Domain.Beverages.Interfaces;
namespace Cafe.Domain.Beverages.Decorators;

public sealed class SyrupDecorator : BeverageDecorator
{
    private const decimal SyrupPrice = 0.25m;

    private string Flavour { get; }

    public SyrupDecorator(IBeverage beverage, string flavour = "vanilla") : base(beverage)
        => Flavour = flavour;

    public override decimal Cost() => _beverage.Cost() + SyrupPrice;

    public override string Describe() => $"{_beverage.Describe()}, {Flavour} syrup";
}
