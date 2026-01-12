using AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport;

namespace AirportTool.Application.Contracts.Services
{
    public interface IScheduleImportService
    {
        Task<ScheduleImportSummaryDto> ImportAsync(Stream jsonStream, CancellationToken ct);
    }
}
