using AirportTool.Application.Contracts;
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
            return await _context.FlightSchedules
                .AsNoTracking()
                .ProjectTo<FlightSchedule>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<FlightSchedule> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.FlightSchedules
                .AsNoTracking()
                .Where(fs => fs.Id == id)
                .ProjectTo<FlightSchedule>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);

            return entity;
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
    }
}
