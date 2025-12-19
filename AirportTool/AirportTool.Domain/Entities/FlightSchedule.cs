using System;

namespace AirportTool.Domain.Entities
{
    public class FlightSchedule
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
        public int? GateId { get; set; }
        public int? AssignedAircraftId { get; set; }
        public int StatusId { get; set; }

        public Gate? Gate { get; set; }
        public Aircraft? AssignedAircraft { get; set; }
        public FlightStatus Status { get; set; } = null!;
    }
}
