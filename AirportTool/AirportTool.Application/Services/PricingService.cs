using AirportTool.Application.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Services
{
    public class PricingService : IPricingService
    {
            public Task<PriceBreakdown> PriceTicketAsync(PriceTicketRequest request, CancellationToken ct = default)
            {
                // super basic rules: base by class, taxes = 10% of base + fixed fee
                var fareClass = char.ToUpperInvariant(request.FareClass);
                var basePrice = fareClass switch
                {
                    'Y' => 100m,
                    'M' => 150m,
                    'J' => 300m,
                    'F' => 500m,
                    _ => 120m
                };

                var taxes = Math.Round(basePrice * 0.10m + 15m, 2, MidpointRounding.AwayFromZero);
                var total = Math.Round(basePrice + taxes, 2, MidpointRounding.AwayFromZero);

                return Task.FromResult(new PriceBreakdown(
                    BasePrice: basePrice,
                    Taxes: taxes,
                    TotalPrice: total,
                    Currency: "EUR",
                    IsRefundable: fareClass is 'J' or 'F'
                ));
            }
        
    }
}
