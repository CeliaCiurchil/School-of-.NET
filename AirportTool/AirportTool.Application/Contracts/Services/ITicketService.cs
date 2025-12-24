using AirportTool.Application.ModelDto.Booking;
using AirportTool.Application.ModelDto.Ticket;
using AirportTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts.Services
{
    public interface ITicketService
    {
        public Task<IEnumerable<TicketReadDto>> GetTicketsByFlightIdAsync(int flightId, CancellationToken ct = default);
        public Task<TicketReadDto> CreateAsync(TicketCreateDto createDto, CancellationToken ct = default);
        public Task<TicketReadDto> UpdateAsync(int id,TicketUpdateDto updateDto, CancellationToken ct = default);
        public Task DeleteAsync(int id, CancellationToken ct = default);
        Task<TicketReadDto> GetByIdAsync(int id, CancellationToken ct);
    }
}
