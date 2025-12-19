namespace AirportTool.Application.ModelDto.FlightSchedule
{
    public class FlightScheduleReadDto : BaseFlightScheduleDto
    {
        public int Id { get; set; }
        public GateDto? Gate { get; set; }
        public AircraftDto? AssignedAircraft { get; set; }
        public FlightStatusDto Status { get; set; } = null!;
    }
}
