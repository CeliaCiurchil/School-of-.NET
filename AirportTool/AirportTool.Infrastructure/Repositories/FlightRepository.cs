using AirportTool.Application.Contracts;
using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using FlightDb = AirportTool.Infrastructure.Persistence.Entities.Flight;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightRepository : IFlightRepository
    {
        private readonly FlightBookingDbContext _context;
        private readonly IMapper _mapper;

        public FlightRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Flight> AddAsync(Flight entity, CancellationToken ct = default)
        {
            var dbEntity = _mapper.Map<FlightDb>(entity);
            await _context.Flights.AddAsync(dbEntity, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<Flight>(dbEntity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _context.Flights
                .Where(f => f.Id == id)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return _context.Flights
                .AsNoTracking()
                .AnyAsync(f => f.Id == id, ct);
        }

        public Task<bool> ExistsByAirlineAndNumberAsync(int airlineId, string flightNumber, CancellationToken ct = default)
        {
            return _context.Flights
                .AsNoTracking()
                .AnyAsync(f => f.AirlineId == airlineId && f.FlightNumber == flightNumber, ct);
        }

        public async Task<IEnumerable<Flight>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Flights
                .AsNoTracking()
                .ProjectTo<Flight>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<Flight> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.Flights
                .AsNoTracking()
                .Where(f => f.Id == id)
                .ProjectTo<Flight>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Flight entity, CancellationToken ct = default)
        {
            await _context.Flights
                .Where(f => f.Id == entity.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(f => f.AirlineId, entity.AirlineId)
                    .SetProperty(f => f.FlightNumber, entity.FlightNumber)
                    .SetProperty(f => f.OriginAirportId, entity.OriginAirportId)
                    .SetProperty(f => f.DestinationAirportId, entity.DestinationAirportId)
                    .SetProperty(f => f.DefaultAircraftId, entity.DefaultAircraftId)
                    .SetProperty(f => f.IsActive, entity.IsActive),
                ct);
        }
    }
}
