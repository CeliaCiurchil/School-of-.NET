using AirportTool.Domain.Entities;

namespace AirportTool.Application.Contracts
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        Task<bool> ExistsByAirlineAndNumberAsync(int airlineId, string flightNumber, CancellationToken ct = default);
    }
}