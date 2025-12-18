using AirportTool.Application.Contracts;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.Flight;
using AirportTool.Domain.Entities;
using AutoMapper;

namespace AirportTool.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FlightReadDto>> GetAllAsync(CancellationToken ct = default)
        {
            var flights = await _unitOfWork.Flights.GetAllAsync(ct);
            return _mapper.Map<IEnumerable<FlightReadDto>>(flights);
        }

        public async Task<FlightReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var flight = await _unitOfWork.Flights.GetByIdAsync(id, ct);
            return flight is null ? throw new NotFoundException(typeof(Flight).Name, id) : _mapper.Map<FlightReadDto>(flight);
        }

        public async Task<FlightReadDto> CreateAsync(FlightCreateDto dto, CancellationToken ct = default)
        {
            var flight = _mapper.Map<Flight>(dto);
            var created = await _unitOfWork.Flights.AddAsync(flight, ct);
            return _mapper.Map<FlightReadDto>(created);
        }

        public async Task<FlightReadDto?> UpdateAsync(int id, FlightUpdateDto dto, CancellationToken ct = default)
        {
            var exists = await _unitOfWork.Flights.ExistsAsync(id, ct);
            if (!exists)
                throw new NotFoundException(typeof(Flight).Name, id);

            var flight = _mapper.Map<Flight>(dto);
            flight.Id = id;

            await _unitOfWork.Flights.UpdateAsync(flight, ct);

            var updated = await _unitOfWork.Flights.GetByIdAsync(id, ct);
            return updated is null ? null : _mapper.Map<FlightReadDto>(updated);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var exists = await _unitOfWork.Flights.ExistsAsync(id, ct);
            if (!exists)
            {
                throw new NotFoundException(typeof(Flight).Name, id);
            }

            await _unitOfWork.Flights.DeleteAsync(id, ct);
            return true;
        }
    }
}
