using AirportTool.Application.Contracts.Repositories;
using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using FlightScheduleDb = AirportTool.Infrastructure.Persistence.Entities.FlightSchedule;

namespace AirportTool.Infrastructure.Repositories
{
    public class FlightScheduleRepository : IFlightScheduleRepository
    {
        private readonly FlightBookingDbContext _context;
        private readonly IMapper _mapper;

        public FlightScheduleRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<FlightSchedule> AddAsync(FlightSchedule entity, CancellationToken ct = default)
        {
            var dbEntity = _mapper.Map<FlightScheduleDb>(entity);
            await _context.FlightSchedules.AddAsync(dbEntity, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<FlightSchedule>(dbEntity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _context.FlightSchedules
                .Where(fs => fs.Id == id)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return _context.FlightSchedules
                .AsNoTracking()
                .AnyAsync(fs => fs.Id == id, ct);
        }

        public async Task<IEnumerable<FlightSchedule>> GetAllAsync(CancellationToken ct = default)
        {
            var entities = await _context.FlightSchedules
                .AsNoTracking()
                .ProjectTo<FlightSchedule>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);

            return entities;
        }

        public async Task<FlightSchedule?> GetByFlightAndDepartureAsync(
            int flightId,
            DateTime scheduledDepartureUtc,
            CancellationToken ct = default)
        {
            var entity = await _context.FlightSchedules
                .AsNoTracking()
                .Where(fs => fs.FlightId == flightId && fs.ScheduledDepartureUtc == scheduledDepartureUtc)
                .ProjectTo<FlightSchedule>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            return entity;
        }

        public async Task<FlightSchedule> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.FlightSchedules
                .AsNoTracking()
                .ProjectTo<FlightSchedule>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(fs => fs.Id == id, ct);

            return entity;
        }

        public async Task<IEnumerable<FlightScheduleBasicInfo>> FindByRouteAndDateAsync(
            string originIata,
            string destinationIata,
            DateTime departureDate,
            CancellationToken ct = default)
        {
            var departureDayStart = departureDate.Date;

            var normalizedOrigin = originIata.Trim().ToUpperInvariant();
            var normalizedDestination = destinationIata.Trim().ToUpperInvariant();

            return await _context.FlightSchedules
                .AsNoTracking()
                .Where(fs => fs.ScheduledDepartureUtc.Date == departureDayStart)
                .Where(fs => fs.Flight.OriginAirport.Iatacode == normalizedOrigin &&
                           fs.Flight.DestinationAirport.Iatacode == normalizedDestination)
                .ProjectTo<FlightScheduleBasicInfo>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task UpdateAsync(FlightSchedule entity, CancellationToken ct = default)
        {
            await _context.FlightSchedules
                .Where(fs => fs.Id == entity.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(fs => fs.FlightId, entity.FlightId)
                    .SetProperty(fs => fs.ScheduledDepartureUtc, entity.ScheduledDepartureUtc)
                    .SetProperty(fs => fs.ScheduledArrivalUtc, entity.ScheduledArrivalUtc)
                    .SetProperty(fs => fs.GateId, entity.GateId)
                    .SetProperty(fs => fs.AssignedAircraftId, entity.AssignedAircraftId)
                    .SetProperty(fs => fs.StatusId, entity.StatusId),
                ct);
        }

        public async Task<IEnumerable<UpcomingFlights>> UpcomingFlights(int days, CancellationToken ct)
        {
            var start = DateTime.UtcNow.Date;
            var endExclusive = start.AddDays(days);

            var existing = await _context.FlightSchedules
                .AsNoTracking()
                .Where(fs => fs.ScheduledDepartureUtc >= start &&
                             fs.ScheduledDepartureUtc < endExclusive)
                .GroupBy(fs => fs.ScheduledDepartureUtc.Date)
                .Select(g => new { Date = g.Key, Count = g.Count() })
                .ToListAsync(ct);

            var byDate = existing.ToDictionary(x => x.Date, x => x.Count);

            var result = Enumerable.Range(0, days)
                .Select(i =>
                {
                    var d = start.AddDays(i);
                    return new UpcomingFlights
                    {
                        Date = d,
                        Count = byDate.TryGetValue(d, out var c) ? c : 0
                    };
                })
                .ToList();

            return result;
        }

        public async Task<bool> HasGateOverlapAsync(int gateId, DateTime scheduledDepartureUtc, int bufferMinutes, CancellationToken ct = default)
        {
            var windowStart = scheduledDepartureUtc.AddMinutes(-bufferMinutes);
            var windowEnd = scheduledDepartureUtc.AddMinutes(bufferMinutes);

            var hasOverlap = await _context.FlightSchedules
                .AsNoTracking()
                .Where(fs => fs.GateId == gateId)
                .AnyAsync(fs => windowStart < fs.ScheduledDepartureUtc.AddMinutes(bufferMinutes) && windowEnd > fs.ScheduledDepartureUtc.AddMinutes(-bufferMinutes), ct);

            return hasOverlap;
        }

        public async Task<bool> HasGateOverlapAsync(
            int gateId,
            DateTime scheduledDepartureUtc,
            int bufferMinutes,
            int? excludeFlightScheduleId,
            CancellationToken ct = default)
        {
            var query = _context.FlightSchedules
                .AsNoTracking()
                .Where(fs => fs.GateId == gateId);

            if (excludeFlightScheduleId.HasValue)
            {
                query = query.Where(fs => fs.Id != excludeFlightScheduleId.Value);
            }

            var windowStart = scheduledDepartureUtc.AddMinutes(-bufferMinutes);
            var windowEnd = scheduledDepartureUtc.AddMinutes(bufferMinutes);

            var hasOverlap = await query.AnyAsync(fs => windowStart < fs.ScheduledDepartureUtc.AddMinutes(bufferMinutes) && windowEnd > fs.ScheduledDepartureUtc.AddMinutes(-bufferMinutes), ct);

            return hasOverlap;
        }

        public Task<int?> GetSeatCapacityAsync(int flightScheduleId, CancellationToken ct = default)
        {
            return _context.FlightSchedules
                .AsNoTracking()
                .Where(fs => fs.Id == flightScheduleId)
                .Select(fs => fs.AssignedAircraft != null ? (int?)fs.AssignedAircraft.SeatCapacity : null)
                .FirstOrDefaultAsync(ct);
        }

    }
}
