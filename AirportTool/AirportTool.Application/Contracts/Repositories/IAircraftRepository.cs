using AirportTool.Domain.Entities;

namespace AirportTool.Application.Contracts.Repositories
{
    public interface IAircraftRepository : IGenericRepository<Aircraft>
    {
        Task<Aircraft?> GetByTailNumberAsync(string tailNumber, CancellationToken ct = default);
    }
}
