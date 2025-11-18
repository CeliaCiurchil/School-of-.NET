using Cafe.Domain.Beverages.Interfaces;

namespace Cafe.Domain.Beverages;

public class Expresso : IBeverage
{
    public string Name => "Expresso";

    public decimal Cost() => 2.50m;

    public string Describe() => "Expresso";
}
