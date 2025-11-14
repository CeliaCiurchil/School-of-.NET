using Cafe.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cafe.Domain.Enums;
using Cafe.Application.Models.Orders;

namespace Cafe.Application.Services;

public interface IOrderService
{
    OrderPlaced PlaceOrder(OrderRequest orderRequest);
}