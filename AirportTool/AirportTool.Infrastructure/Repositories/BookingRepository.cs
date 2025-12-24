using AirportTool.Application.Contracts.Repositories;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Enums;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using BookingDb = AirportTool.Infrastructure.Persistence.Entities.Booking;

namespace AirportTool.Infrastructure.Repositories
{
    public class BookingRepository : IBookingRepository
    {
        private readonly FlightBookingDbContext _context;
        private readonly IMapper _mapper;

        public BookingRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Booking> AddAsync(Booking entity, CancellationToken ct = default)
        {
            var dbEntity = _mapper.Map<BookingDb>(entity);
            await _context.Bookings.AddAsync(dbEntity, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<Booking>(dbEntity);
        }

        public async Task CancelAsync(string confirmationCode, CancellationToken ct)
        {
            await _context.Bookings
                .Where(b => b.ConfirmationCode == confirmationCode)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(b => b.BookingStatusId, (int)BookingStatus.Cancelled)
                    .SetProperty(b => b.Quantity, 0),
                ct);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var bookingId = (long)id;
            await _context.Bookings
                .Where(b => b.Id == bookingId)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            var bookingId = (long)id;
            return _context.Bookings
                .AsNoTracking()
                .AnyAsync(b => b.Id == bookingId, ct);
        }

        public async Task<bool> ExistsAsync(string confirmationCode, CancellationToken ct = default)
        {
            return await _context.Bookings
                .AsNoTracking()
                .AnyAsync(b => b.ConfirmationCode == confirmationCode, ct);
        }

        public async Task<IEnumerable<Booking>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Bookings
                .AsNoTracking()
                .ProjectTo<Booking>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public Task<Booking?> GetByCodeAsync(string confirmationCode, CancellationToken ct)
        {
            return _context.Bookings
                .AsNoTracking()
                .Where(b => b.ConfirmationCode == confirmationCode)
                .ProjectTo<Booking>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
        }

        public async Task<Booking> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var bookingId = (long)id;
            var entity = await _context.Bookings
                .AsNoTracking()
                .Where(b => b.Id == bookingId)
                .ProjectTo<Booking>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Booking entity, CancellationToken ct = default)
        {
            await _context.Bookings
                .Where(b => b.Id == entity.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(b => b.UserId, entity.UserId)
                    .SetProperty(b => b.BookingStatusId, entity.BookingStatusId)
                    .SetProperty(b => b.CreatedUtc, entity.CreatedUtc)
                    .SetProperty(b => b.ConfirmationCode, entity.ConfirmationCode)
                    .SetProperty(b => b.Quantity, entity.Quantity),
                ct);
        }
    }
}