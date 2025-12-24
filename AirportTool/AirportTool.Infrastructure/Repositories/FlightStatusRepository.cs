using AirportTool.Application.Contracts.Repositories;
using AirportTool.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightStatusRepository : IFlightStatusRepository
    {
        private readonly FlightBookingDbContext _context;

        public FlightStatusRepository(FlightBookingDbContext context)
        {
            _context = context;
        }

        public Task<int?> GetIdByStatusAsync(string status, CancellationToken ct = default)
        {
            return _context.FlightStatuses
                .AsNoTracking()
                .Where(s => s.Status == status)
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync(ct);
        }
    }
}
