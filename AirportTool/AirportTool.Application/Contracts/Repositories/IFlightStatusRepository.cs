namespace AirportTool.Application.Contracts.Repositories
{
    public interface IFlightStatusRepository
    {
        Task<int?> GetIdByStatusAsync(string status, CancellationToken ct = default);
    }
}
