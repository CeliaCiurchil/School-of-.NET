namespace AirportTool.Application.Contracts.Repositories
{
    public interface IGateRepository
    {
        Task<int?> GetGateIdByCodeAndAirportAsync(int airportId, string gateCode, CancellationToken ct = default);
    }
}
