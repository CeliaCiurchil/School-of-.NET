using Cafe.Domain.Beverages;
using Cafe.Domain.Beverages.Decorators;
using Cafe.Domain.Beverages.Interfaces;
using Xunit;

namespace Cafe.Tests.Beverages.Decorators
{
    public class MilkDecoratorTests
    {
        [Fact]
        public void Cost_ExpressoWithMilk_IsBasePlusMilk()
        {
            IBeverage baseDrink = new Expresso();   
            IBeverage withMilk = new MilkDecorator(baseDrink); 

            decimal cost = withMilk.Cost();

            Assert.Equal(2.90m, cost); 
        }

        [Fact]
        public void Describe_ExpressoWithMilk_AppendsMilk()
        {
            IBeverage withMilk = new MilkDecorator(new Expresso());

            string description = withMilk.Describe();

            Assert.Contains("milk", description.ToLowerInvariant());
            Assert.StartsWith("Expresso", description); 
        }
    }
}
