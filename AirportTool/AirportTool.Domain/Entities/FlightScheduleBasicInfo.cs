using System;

namespace AirportTool.Domain.Entities
{
    public class FlightScheduleBasicInfo
    {
        public int ScheduleId { get; set; }
        public int FlightId { get; set; }
        public string FlightNumber { get; set; } = null!;
        public string AirlineCode { get; set; } = null!;
        public string OriginAirportCode { get; set; } = null!;
        public string DestinationAirportCode { get; set; } = null!;
        public DateTime ScheduledDepartureUtc { get; set; }
        public DateTime ScheduledArrivalUtc { get; set; }
    }
}
