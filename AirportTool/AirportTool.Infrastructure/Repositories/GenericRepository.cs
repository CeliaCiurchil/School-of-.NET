using AirportTool.Application.Contracts;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, T> : IGenericRepository<T> where T : class, new() where TEntity : class, new()
    {
        private readonly IMapper _mapper;
        private readonly FlightBookingDbContext _context;

        public GenericRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddAsync(T domainModel)
        {
            var entity = _mapper.Map<TEntity>(domainModel);
            await _context.AddAsync(entity);
        }

        public Task<T> AddAsync(T entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Exists(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<T>> GetAllAsync(CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public async Task<T> GetByIdAsync(int id, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(T entity, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
