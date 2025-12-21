namespace AirportTool.Application.ModelDto.Flight
{
    public class BaseFlightDto
    {
        public string AirlineIata { get; set; } = null!;
        public string FlightNumber { get; set; } = null!;
        public string OriginIata { get; set; } = null!;
        public string DestinationIata { get; set; } = null!;
        public string? DefaultAircraftTail { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
