using Cafe.Domain.Beverages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Beverages.Decorators
{
    public sealed class SyrupDecorator : BeverageDecorator
    {
        private const decimal SyrupPrice = 0.25m;
        private string Flavour { get; }

        public SyrupDecorator(IBeverage beverage, string flavour) : base(beverage) 
        {
            Flavour = flavour;
        }

        public override decimal Cost()
        {
            return _beverage.Cost() + SyrupPrice;
        }

        public override string Describe()
        {
            return $"{_beverage.Describe()}, {Flavour} syrup";
        }
    }
}
