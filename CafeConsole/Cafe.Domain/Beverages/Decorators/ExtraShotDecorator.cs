using Cafe.Domain.Beverages.Interfaces;

namespace Cafe.Domain.Beverages.Decorators;

public sealed class ExtraShotDecorator : BeverageDecorator
{
    private const decimal ExtraShotPrice = 0.80m;

    public ExtraShotDecorator(IBeverage beverage) : base(beverage) { }

    public override decimal Cost() => base.Cost() + ExtraShotPrice;

    public override string Describe() => $"{base.Describe()}, extra shot of expresso";
}
