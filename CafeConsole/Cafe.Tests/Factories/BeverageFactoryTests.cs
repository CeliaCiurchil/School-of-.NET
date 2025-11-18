using Cafe.Domain.Beverages;
using Cafe.Domain.Beverages.Interfaces;
using Cafe.Domain.Enums;
using Cafe.Infrastructure.Factories;

namespace Cafe.Tests.Factories;

public class BeverageFactoryTests
{
    [Fact]
    public void CreateBeverage_WithExpressoType_ReturnsExpressoInstance()
    {
        var factory = new BeverageFactory();

        IBeverage beverage = factory.CreateBeverage(BeverageType.Expresso);

        Assert.IsType<Expresso>(beverage);
    }

    [Fact]
    public void CreateBeverage_WithTeaType_ReturnsTeaInstance()
    {
        var factory = new BeverageFactory();

        IBeverage beverage = factory.CreateBeverage(BeverageType.Tea);

        Assert.IsType<Tea>(beverage);
    }

    [Fact]
    public void CreateBeverage_WithInvalidType_ThrowsArgumentException()
    {
        var factory = new BeverageFactory();

        var invalidType = (BeverageType)999;

        Assert.Throws<ArgumentException>(() => factory.CreateBeverage(invalidType));
    }
}
