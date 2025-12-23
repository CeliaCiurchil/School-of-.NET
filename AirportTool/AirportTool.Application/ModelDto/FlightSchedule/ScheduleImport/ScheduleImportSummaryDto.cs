using System.Collections.Generic;

namespace AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport
{
    public class ScheduleImportSummaryDto
    {
        public int Total { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }
        public List<ScheduleImportErrorDto> Errors { get; set; } = new();
        public List<ScheduleImportRowResultDto> Results { get; set; } = new();
    }
}
