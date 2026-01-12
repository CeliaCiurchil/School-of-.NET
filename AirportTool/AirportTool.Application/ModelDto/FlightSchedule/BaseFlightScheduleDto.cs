using System;

namespace AirportTool.Application.ModelDto.FlightSchedule
{
    public class BaseFlightScheduleDto
    {
        public int FlightId { get; set; }
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
        public int? GateId { get; set; }
        public int? AssignedAircraftId { get; set; }
        public int StatusId { get; set; }
    }
}
