using Cafe.Domain.Beverages;
using Cafe.Domain.Beverages.Decorators;
using Cafe.Domain.Beverages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Tests.Beverages.Decorators
{
    public class ExtraShotDecoratorTests
    {
        [Fact]
        public void Cost_ExpressoWithExtraShot_IsBasePlusExtraShot()
        {
            IBeverage baseDrink = new Expresso();          
            IBeverage withExtraShot = new ExtraShotDecorator(baseDrink); 

            decimal cost = withExtraShot.Cost();

            Assert.Equal(3.30m, cost); 
        }

        [Fact]
        public void Describe_ExpressoWithExtraShot_AppendsExtraShot()
        {
            IBeverage withExtraShot = new ExtraShotDecorator(new Expresso());

            string description = withExtraShot.Describe();

            Assert.Contains("extra shot", description.ToLowerInvariant());
        }
    }
}
