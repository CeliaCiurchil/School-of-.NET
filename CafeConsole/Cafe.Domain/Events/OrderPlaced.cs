using Cafe.Domain.Enums;

namespace Cafe.Domain.Events;

public readonly record struct OrderPlaced
(
    Guid OrderId,
    DateTimeOffset PlacedAt,
    string Description,
    decimal Subtotal,
    PricingType PricingType,
    decimal Total
);
