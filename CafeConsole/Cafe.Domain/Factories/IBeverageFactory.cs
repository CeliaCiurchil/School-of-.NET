using Cafe.Domain.Beverages.Interfaces;
using Cafe.Domain.Enums;

namespace Cafe.Domain.Factories;

public interface IBeverageFactory
{
    public IBeverage CreateBeverage(BeverageType beverageType);
}
