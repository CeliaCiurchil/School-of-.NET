using AirportTool.Domain.Entities;

namespace AirportTool.Application.Contracts.Repositories
{
    public interface IAirportRepository : IGenericRepository<Airport>
    {
        Task<Airport?> GetByIataCodeAsync(string iataCode, CancellationToken ct = default);
    }
}
