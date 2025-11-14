using Cafe.Domain.Beverages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Beverages.Decorators
{
    public abstract class BeverageDecorator : IBeverage
    {
        protected readonly IBeverage _beverage;

        protected BeverageDecorator(IBeverage beverage)
            => _beverage = beverage ?? throw new ArgumentNullException(nameof(beverage));

        public virtual string Name => _beverage.Name;

        public virtual decimal Cost() => _beverage.Cost();

        public virtual string Describe() => _beverage.Describe();
    }
}
