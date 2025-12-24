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
            var airline = await GetAirlineByIataAsync(dto.AirlineIata, ct);
            var origin = await GetAirportByIataAsync(dto.OriginIata, ct);
            var destination = await GetAirportByIataAsync(dto.DestinationIata, ct);
            var defaultAircraft = await GetAircraftByTailAsync(dto.DefaultAircraftTail, ct);

            var duplicateExists = await _unitOfWork.Flights.ExistsByAirlineAndNumberAsync(airline.Id, dto.FlightNumber, ct);
            if (duplicateExists)
            {
                throw new ConflictException($"Flight number \"{dto.FlightNumber}\" already exists for airline ID {airline.Id}.");
            }

            var flight = new Flight
            {
                AirlineId = airline.Id,
                FlightNumber = dto.FlightNumber,
                OriginAirportId = origin.Id,
                DestinationAirportId = destination.Id,
                DefaultAircraftId = defaultAircraft?.Id,
                IsActive = dto.IsActive
            };

            var created = await _unitOfWork.Flights.AddAsync(flight, ct);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<FlightReadDto>(created);
        }

        public async Task<FlightReadDto?> UpdateAsync(int id, FlightUpdateDto dto, CancellationToken ct)
        {
            var existing = await _unitOfWork.Flights.GetByIdAsync(id, ct);
            if (existing is null)
            {
                throw new NotFoundException(nameof(Flight), id);
            }

            var airline = await GetAirlineByIataAsync(dto.AirlineIata, ct);
            var origin = await GetAirportByIataAsync(dto.OriginIata, ct);
            var destination = await GetAirportByIataAsync(dto.DestinationIata, ct);
            var defaultAircraft = await GetAircraftByTailAsync(dto.DefaultAircraftTail, ct);

            if (existing.AirlineId != airline.Id ||
                !string.Equals(existing.FlightNumber, dto.FlightNumber, StringComparison.OrdinalIgnoreCase))
            {
                var duplicateExists = await _unitOfWork.Flights.ExistsByAirlineAndNumberAsync(airline.Id, dto.FlightNumber, ct);
                if (duplicateExists)
                {
                    throw new ConflictException($"Flight number \"{dto.FlightNumber}\" already exists for airline ID {airline.Id}.");
                }
            }

            var flight = new Flight
            {
                Id = id,
                AirlineId = airline.Id,
                FlightNumber = dto.FlightNumber,
                OriginAirportId = origin.Id,
                DestinationAirportId = destination.Id,
                DefaultAircraftId = defaultAircraft?.Id,
                IsActive = dto.IsActive
            };

            await _unitOfWork.Flights.UpdateAsync(flight, ct);

            return _mapper.Map<FlightReadDto>(flight);
        }

        public async Task<bool> DeleteAsync(int id, CancellationToken ct = default)
        {
            var exists = await _unitOfWork.Flights.ExistsAsync(id, ct);
            if (!exists)
            {
                throw new NotFoundException(nameof(Flight), id);
            }

            await _unitOfWork.Flights.DeleteAsync(id, ct);
            return true;
        }

        private async Task<Airline> GetAirlineByIataAsync(string iataCode, CancellationToken ct)
        {
            var airline = await _unitOfWork.Airlines.GetByIataCodeAsync(iataCode, ct);
            return EnsureFound(airline, iataCode);
        }

        private async Task<Airport> GetAirportByIataAsync(string iataCode, CancellationToken ct)
        {
            var airport = await _unitOfWork.Airports.GetByIataCodeAsync(iataCode, ct);
            return EnsureFound(airport, iataCode);
        }

        private async Task<Aircraft?> GetAircraftByTailAsync(string? tailNumber, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(tailNumber))
            {
                return null;
            }

            var aircraft = await _unitOfWork.Aircrafts.GetByTailNumberAsync(tailNumber, ct);
            return EnsureFound(aircraft, tailNumber);
        }

        private static T EnsureFound<T>(T? entity, object key) where T : class
        {
            return entity ?? throw new BadRequestException($"{typeof(T).Name} \"{key}\" is invalid / not found.");
        }
    }
}
