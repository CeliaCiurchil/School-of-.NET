using AirportTool.Domain.Entities;

namespace AirportTool.Application.Contracts
{
    public interface IAircraftRepository : IGenericRepository<Aircraft>
    {
        Task<Aircraft?> GetByTailNumberAsync(string tailNumber, CancellationToken ct = default);
    }
}
