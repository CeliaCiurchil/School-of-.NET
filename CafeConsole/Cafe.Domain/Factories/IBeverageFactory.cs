using Cafe.Domain.Beverages;
using Cafe.Domain.Beverages.Interfaces;
using Cafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Factories
{
    public interface IBeverageFactory
    {
        public IBeverage CreateBeverage(BeverageType beverageType);
    }
}
