using AirportTool.Domain.Entities;

namespace AirportTool.Application.Contracts
{
    public interface IAirlineRepository : IGenericRepository<Airline>
    {
        Task<Airline?> GetByIataCodeAsync(string iataCode, CancellationToken ct = default);
    }
}
