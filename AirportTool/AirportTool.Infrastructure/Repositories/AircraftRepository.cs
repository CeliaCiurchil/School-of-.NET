using AirportTool.Application.Contracts;
using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AircraftDb = AirportTool.Infrastructure.Persistence.Entities.Aircraft;

namespace AirportTool.Infrastructure.Repositories
{
    public class AircraftRepository : IAircraftRepository
    {
        private readonly FlightBookingDbContext _context;
        private readonly IMapper _mapper;

        public AircraftRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Aircraft> AddAsync(Aircraft entity, CancellationToken ct = default)
        {
            var dbEntity = _mapper.Map<AircraftDb>(entity);
            await _context.Aircraft.AddAsync(dbEntity, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<Aircraft>(dbEntity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _context.Aircraft
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return _context.Aircraft
                .AsNoTracking()
                .AnyAsync(a => a.Id == id, ct);
        }

        public async Task<IEnumerable<Aircraft>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Aircraft
                .AsNoTracking()
                .ProjectTo<Aircraft>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<Aircraft> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.Aircraft
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ProjectTo<Aircraft>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
            return entity;
        }

        public Task<Aircraft> GetByTailNumberAsync(string tailNumber)
        {
            var entity = _context.Aircraft
                .AsNoTracking()
                .Where(a => a.TailNumber == tailNumber)
                .ProjectTo<Aircraft>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

            return entity;
        }

        public async Task UpdateAsync(Aircraft entity, CancellationToken ct = default)
        {
            await _context.Aircraft
                .Where(a => a.Id == entity.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.TailNumber, entity.TailNumber)
                    .SetProperty(a => a.Model, entity.Model)
                    .SetProperty(a => a.SeatCapacity, entity.SeatCapacity)
                    .SetProperty(a => a.AirlineId, entity.AirlineId),
                ct);
        }
    }
}
