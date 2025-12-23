namespace AirportTool.Application.Contracts
{
    public interface IFlightStatusRepository
    {
        Task<int?> GetIdByStatusAsync(string status, CancellationToken ct = default);
    }
}
