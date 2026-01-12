using System;

namespace AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport
{
    public class ScheduleImportRowDto
    {
        public string? FlightNumber { get; set; }
        public string? AirlineIata { get; set; }
        public string? OriginIata { get; set; }
        public string? DestinationIata { get; set; }
        public DateTimeOffset? ScheduledDepartureUtc { get; set; }
        public DateTimeOffset? ScheduledArrivalUtc { get; set; }
        public string? GateCode { get; set; }
        public string? AssignedAircraftTail { get; set; }
    }
}
