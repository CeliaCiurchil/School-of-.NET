namespace AirportTool.Application.Contracts
{
    public interface IGateRepository
    {
        Task<int?> GetGateIdByCodeAndAirportAsync(int airportId, string gateCode, CancellationToken ct = default);
    }
}
