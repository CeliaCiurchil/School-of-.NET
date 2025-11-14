using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Application.Services;
using Cafe.Domain.Enums;
using Cafe.Domain.Factories;

public class OrderService
{
    IBeverageFactory _beverageFactory;
    public OrderService(IBeverageFactory beverageFactory)
    {
        _beverageFactory = beverageFactory;
    }
    public void ChooseBase(BeverageType beverageType)
    {
        var beverage = _beverageFactory.CreateBeverage(beverageType);
        Console.WriteLine(beverage.Describe());
    }
}
