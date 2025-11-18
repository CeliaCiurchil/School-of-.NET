using Cafe.Domain.Beverages.Interfaces;

namespace Cafe.Domain.Beverages;
public class Tea : IBeverage
{
    public string Name => "Tea";

    public decimal Cost() => 2.00m;

    public string Describe() => "Tea";
}
