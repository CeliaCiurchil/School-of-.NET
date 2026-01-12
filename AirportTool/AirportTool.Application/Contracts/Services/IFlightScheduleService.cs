using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.Application.ModelDto.FlightSchedule.Stats;

namespace AirportTool.Application.Contracts.Services
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
