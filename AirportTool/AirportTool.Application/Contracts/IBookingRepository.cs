using AirportTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        Task CancelAsync(string confirmationCode, CancellationToken ct);
        Task<bool> ExistsAsync(string confirmationCode, CancellationToken ct = default);
        Task<Booking?> GetByCodeAsync(string confirmationCode, CancellationToken ct);
    }
}
