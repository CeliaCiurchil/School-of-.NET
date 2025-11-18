using Cafe.Domain.Enums;

namespace Cafe.Application.Models.Orders;

public sealed record OrderRequest(
    BeverageType BaseDrink,
    IReadOnlyList<AddOnChoice> AddOns,
    PricingType PricingPolicy
);
