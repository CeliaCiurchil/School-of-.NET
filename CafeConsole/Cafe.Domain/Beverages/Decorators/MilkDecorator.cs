using Cafe.Domain.Beverages.Interfaces;
namespace Cafe.Domain.Beverages.Decorators;

public sealed class MilkDecorator : BeverageDecorator
{
    private const decimal MilkPrice = 0.40m;

    public MilkDecorator(IBeverage beverage) : base(beverage) { }

    public override decimal Cost() => base.Cost() + MilkPrice;

    public override string Describe() => $"{base.Describe()}, milk";
}
