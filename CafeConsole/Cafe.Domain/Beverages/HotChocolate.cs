using Cafe.Domain.Beverages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Beverages
{
    public class HotChocolate : IBeverage
    {
        public string Name => "Hot Chocolate";

        public decimal Cost() => 3.00m;

        public string Describe() => "Hot chocolate";
    }
}
