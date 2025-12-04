using Cafe.Application.Models.Orders;
using Cafe.Domain.Events;

namespace Cafe.Application.Services;

public interface IOrderService
{
    OrderPlaced PlaceOrder(OrderRequest orderRequest);
}
