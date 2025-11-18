using Cafe.Domain.Beverages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Beverages;

public class Expresso : IBeverage
{
    public string Name => "Expresso";

    public decimal Cost() => 2.50m;

    public string Describe() => "Expresso";
}
