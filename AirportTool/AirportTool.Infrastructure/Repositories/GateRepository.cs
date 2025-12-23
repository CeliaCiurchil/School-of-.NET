using AirportTool.Application.Contracts;
using AirportTool.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class GateRepository : IGateRepository
    {
        private readonly FlightBookingDbContext _context;

        public GateRepository(FlightBookingDbContext context)
        {
            _context = context;
        }

        public Task<int?> GetGateIdByCodeAndAirportAsync(int airportId, string gateCode, CancellationToken ct = default)
        {
            return _context.Gates
                .AsNoTracking()
                .Where(g => g.AirportId == airportId && g.Code == gateCode)
                .Select(g => (int?)g.Id)
                .FirstOrDefaultAsync(ct);
        }
    }
}
