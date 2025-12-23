using AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport;

namespace AirportTool.Application.Contracts
{
    public interface IScheduleImportService
    {
        Task<ScheduleImportSummaryDto> ImportAsync(Stream jsonStream, CancellationToken ct);
    }
}
