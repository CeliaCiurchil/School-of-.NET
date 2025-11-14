using Cafe.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cafe.Application.Models.Orders;

public sealed record OrderRequest(
    BeverageType BaseDrink,
    IReadOnlyList<AddOnChoice> AddOns,
    PricingType PricingPolicy
);

