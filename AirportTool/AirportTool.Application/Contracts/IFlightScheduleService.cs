using AirportTool.Application.ModelDto.FlightSchedule;

namespace AirportTool.Application.Contracts
{
    public interface IFlightScheduleService
    {
        Task<FlightScheduleReadDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FlightScheduleReadDto> CreateAsync(FlightScheduleCreateDto dto, CancellationToken ct = default);
        Task<IEnumerable<FlightScheduleBasicInfoDto>> FindByRouteAndDateAsync(
            string origin,
            string destination,
            DateTime departureDate,
            CancellationToken ct = default);
        Task<IEnumerable<UpcomingFlightsDto>> GetFlightStats(int days, CancellationToken ct);
    }
}
