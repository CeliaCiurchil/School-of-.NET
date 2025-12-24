using AirportTool.Application.Contracts.Repositories;
using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using TicketDb = AirportTool.Infrastructure.Persistence.Entities.Ticket;

namespace AirportTool.Infrastructure.Repositories
{
    public class TicketRepository : ITicketRepository
    {
        private readonly FlightBookingDbContext _context;
        private readonly IMapper _mapper;

        public TicketRepository(FlightBookingDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Ticket> AddAsync(Ticket entity, CancellationToken ct = default)
        {
            var dbEntity = _mapper.Map<TicketDb>(entity);
            await _context.Tickets.AddAsync(dbEntity, ct);
            await _context.SaveChangesAsync(ct);
            return _mapper.Map<Ticket>(dbEntity);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var ticketId = (long)id;
            await _context.Tickets
                .Where(t => t.Id == ticketId)
                .ExecuteDeleteAsync(ct);
        }

        public Task<bool> ExistsAsync(int id, CancellationToken ct = default)
        {
            var ticketId = (long)id;
            return _context.Tickets
                .AsNoTracking()
                .AnyAsync(t => t.Id == ticketId, ct);
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(CancellationToken ct = default)
        {
            return await _context.Tickets
                .AsNoTracking()
                .ProjectTo<Ticket>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<IEnumerable<Ticket>> GetByFlightIdAsync(int flightId, CancellationToken ct = default)
        {
            return await _context.Tickets
                .AsNoTracking()
                .Where(t => t.FlightSchedule.FlightId == flightId)
                .ProjectTo<Ticket>(_mapper.ConfigurationProvider)
                .ToListAsync(ct);
        }

        public async Task<Ticket> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var ticketId = (long)id;
            var entity = await _context.Tickets
                .AsNoTracking()
                .Where(t => t.Id == ticketId)
                .ProjectTo<Ticket>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(ct);
            return entity;
        }

        public async Task UpdateAsync(Ticket entity, CancellationToken ct = default)
        {
            await _context.Tickets
                .Where(t => t.Id == entity.Id)
                .ExecuteUpdateAsync(setters => setters
                    .SetProperty(t => t.BookingId, entity.BookingId)
                    .SetProperty(t => t.FlightScheduleId, entity.FlightScheduleId)
                    .SetProperty(t => t.FareClass, entity.FareClass)
                    .SetProperty(t => t.BasePrice, entity.BasePrice)
                    .SetProperty(t => t.Taxes, entity.Taxes)
                    .SetProperty(t => t.TotalPrice, entity.TotalPrice)
                    .SetProperty(t => t.Currency, entity.Currency)
                    .SetProperty(t => t.IsRefundable, entity.IsRefundable)
                    .SetProperty(t => t.SeatNumber, entity.SeatNumber)
                    .SetProperty(t => t.PassengerFullName, entity.PassengerFullName)
                    .SetProperty(t => t.PassengerEmail, entity.PassengerEmail),
                ct);
        }
    }
}