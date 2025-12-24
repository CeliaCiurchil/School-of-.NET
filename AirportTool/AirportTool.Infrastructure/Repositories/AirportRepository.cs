using AirportTool.Application.Contracts.Repositories;
using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using AirportDb = AirportTool.Infrastructure.Persistence.Entities.Airport;

namespace AirportTool.Infrastructure.Repositories
{
    public class AirportRepository : IAirportRepository
    {
        private readonly FlightBookingDbContext _context;
        private readonly IMapper _mapper;

        public AirportRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Airport> AddAsync(Airport entity, CancellationToken ct = default)
        {
            var dbEntity = _mapper.Map<AirportDb>(entity);
            await _context.Airports.AddAsync(dbEntity, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<Airport>(dbEntity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            await _context.Airports
                .Where(a => a.Id == id)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            return _context.Airports
                .AsNoTracking()
                .AnyAsync(a => a.Id == id, ct);
        }

        public async Task<IEnumerable<Airport>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Airports
                .AsNoTracking()
                .ProjectTo<Airport>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<Airport> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var entity = await _context.Airports
                .AsNoTracking()
                .Where(a => a.Id == id)
                .ProjectTo<Airport>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
            return entity;
        }

        public Task<Airport?> GetByIataCodeAsync(string iataCode, CancellationToken ct = default)
        {
            var entity = _context.Airports
                .AsNoTracking()
                .Where(a => a.Iatacode == iataCode)
                .ProjectTo<Airport>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Airport entity, CancellationToken ct = default)
        {
            await _context.Airports
                .Where(a => a.Id == entity.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(a => a.Iatacode, entity.Iatacode)
                    .SetProperty(a => a.Name, entity.Name)
                    .SetProperty(a => a.TimeZone, entity.TimeZone)
                    .SetProperty(a => a.AddressId, entity.AddressId),
                ct);
        }
    }
}
