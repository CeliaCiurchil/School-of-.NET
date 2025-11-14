using Cafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Domain.Events
{
    public readonly record struct OrderPlaced
    (
        Guid OrderId,
        DateTimeOffset PlacedAt,
        string Description,
        decimal Subtotal,
        PricingType PricingType,
        decimal Total
    );
}
