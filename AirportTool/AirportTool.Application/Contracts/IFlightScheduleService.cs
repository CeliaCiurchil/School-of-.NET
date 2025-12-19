using AirportTool.Application.ModelDto.FlightSchedule;

namespace AirportTool.Application.Contracts
{
    public interface IFlightScheduleService
    {
        Task<FlightScheduleReadDto> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FlightScheduleReadDto> CreateAsync(FlightScheduleCreateDto dto, CancellationToken ct = default);
    }
}
