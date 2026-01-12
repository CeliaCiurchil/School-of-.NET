using AirportTool.Application.ModelDto.Flight;

namespace AirportTool.Application.Contracts.Services
{
    public interface IFlightService
    {
        Task<IEnumerable<FlightReadDto>> GetAllAsync(CancellationToken ct = default);
        Task<FlightReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
        Task<FlightReadDto> CreateAsync(FlightCreateDto dto, CancellationToken ct = default);
        Task<FlightReadDto?> UpdateAsync(int id, FlightUpdateDto dto, CancellationToken ct = default);
        Task<bool> DeleteAsync(int id, CancellationToken ct = default);
    }
}
