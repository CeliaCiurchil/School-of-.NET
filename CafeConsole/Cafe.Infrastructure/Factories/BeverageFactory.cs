using Cafe.Domain.Beverages;
using Cafe.Domain.Beverages.Interfaces;
using Cafe.Domain.Enums;
using Cafe.Domain.Factories;

namespace Cafe.Infrastructure.Factories;

public class BeverageFactory : IBeverageFactory
{
    public IBeverage CreateBeverage(BeverageType beverageType)
    {
        switch (beverageType)
        {
            case BeverageType.Expresso:
                return new Expresso();

            case BeverageType.Tea:
                return new Tea();

            case BeverageType.Choc:
                return new HotChocolate();

            default:
                throw new ArgumentException("Invalid beverage type", nameof(beverageType));
        }
    }
}
