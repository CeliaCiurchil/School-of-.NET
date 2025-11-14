using Cafe.Domain.Beverages.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Beverages;
public class Tea : IBeverage
{
    public string Name => "Tea";
    public decimal Cost()
    {
        return 2.00m;
    }
    public string Describe()
    {
        return "Tea";
    }
}