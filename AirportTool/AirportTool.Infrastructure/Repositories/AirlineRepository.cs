using AirportTool.Application.Contracts;
using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AirlineDb = AirportTool.Infrastructure.Persistence.Entities.Airline;

namespace AirportTool.Infrastructure.Repositories
{
    public class AirlineRepository : IAirlineRepository
    {
        private readonly FlightBookingDbContext _context;
        private readonly IMapper _mapper;

        public AirlineRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Airline> AddAsync(Airline entity, CancellationToken ct = default)
        {
            var dbEntity = _mapper.Map<AirlineDb>(entity);
            await _context.Airlines.AddAsync(dbEntity, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<Airline>(dbEntity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _context.Airlines
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return _context.Airlines
                .AsNoTracking()
                .AnyAsync(a => a.Id == id, ct);
        }

        public async Task<IEnumerable<Airline>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Airlines
                .AsNoTracking()
                .ProjectTo<Airline>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<Airline> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.Airlines
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ProjectTo<Airline>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
            return entity;
        }

        public Task<Airline> GetByIataCodeAsync(string iataCode)
        {
            var entity = _context.Airlines
                .AsNoTracking()
                .Where(a => a.Iatacode == iataCode)
                .ProjectTo<Airline>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();
            return entity;
        }

        public async Task UpdateAsync(Airline entity, CancellationToken ct = default)
        {
            await _context.Airlines
                .Where(a => a.Id == entity.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.Iatacode, entity.Iatacode)
                    .SetProperty(a => a.Name, entity.Name),
                ct);
        }
    }
}
