namespace AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport
{
    public class ScheduleImportRowResultDto
    {
        public int Row { get; set; }
        public string Status { get; set; } = string.Empty;
        public int? FlightId { get; set; }
        public int? FlightScheduleId { get; set; }
        public string? Message { get; set; }
    }
}