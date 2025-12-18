using AirportTool.Application.Contracts;
using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public Task AddAsync(Flight entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Flight>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
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

        public Task UpdateAsync(Flight entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
