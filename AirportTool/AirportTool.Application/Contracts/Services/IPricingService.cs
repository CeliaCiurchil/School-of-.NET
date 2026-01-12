using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts.Services
{
    public interface IPricingService
    {
        Task<PriceBreakdown> PriceTicketAsync(PriceTicketRequest request, CancellationToken ct = default);
    }
    public sealed record PriceTicketRequest(
        char FareClass
    );
    public sealed record PriceBreakdown(
        decimal BasePrice,
        decimal Taxes,
        decimal TotalPrice,
        string Currency,
        bool IsRefundable
    );
}
