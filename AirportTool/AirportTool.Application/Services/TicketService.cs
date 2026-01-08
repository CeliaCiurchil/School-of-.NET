using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Services;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.Booking;
using AirportTool.Application.ModelDto.Ticket;
using AirportTool.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPricingService _pricingService;
        private readonly IMapper _mapper;

        public TicketService(IUnitOfWork unitOfWork, IPricingService pricingService, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _pricingService = pricingService;
            _mapper = mapper;
        }

        public async Task<TicketReadDto> CreateAsync(TicketCreateDto createDto, CancellationToken ct = default)
        {
            var capacity = await _unitOfWork.FlightSchedules.GetSeatCapacityAsync(createDto.FlightScheduleId, ct);
            if (capacity is null)
                throw new BadRequestException("Flight schedule has no aircraft assigned, cannot sell tickets.");

            var sold = await _unitOfWork.Tickets.CountByFlightScheduleIdAsync(createDto.FlightScheduleId, ct);

            if (sold + 1 > capacity)
                throw new BadRequestException("No seats available for this flight.");

            var prices = await _pricingService.PriceTicketAsync(
                new PriceTicketRequest(
                    FareClass: createDto.FareClass[0]
                ),
                ct);

            var ticket = _mapper.Map<Ticket>(createDto);
            ticket.BasePrice = prices.BasePrice;
            ticket.Taxes = prices.Taxes;
            ticket.TotalPrice = prices.TotalPrice;
            ticket.Currency = prices.Currency; 
            ticket.IsRefundable = prices.IsRefundable; 

            var createdTicket = await _unitOfWork.Tickets.AddAsync(ticket, ct);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TicketReadDto>(createdTicket);
        }

        public async Task DeleteAsync(int id, CancellationToken ct = default)
        {
            var exists = await _unitOfWork.Tickets.ExistsAsync(id, ct);
            if (!exists)
            {
                throw new NotFoundException(nameof(Ticket), id);
            }

            await _unitOfWork.Tickets.DeleteAsync(id, ct);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<TicketReadDto> GetByIdAsync(int id, CancellationToken ct)
        {
            var ticket = await _unitOfWork.Tickets.GetByIdAsync(id);
            var ticketdto = ticket is not null
                ? _mapper.Map<TicketReadDto>(ticket)
                : null;
            return ticketdto ?? throw new NotFoundException(typeof(Ticket).Name, id);
        
        }

        public async Task<IEnumerable<TicketReadDto>> GetTicketsByFlightIdAsync(int flightId, CancellationToken ct = default)
        {
            //check if flight exists
            var tickets = await _unitOfWork.Tickets.GetByFlightIdAsync(flightId, ct);
            return _mapper.Map<IEnumerable<TicketReadDto>>(tickets);
        }

        public async Task<TicketReadDto> UpdateAsync(int id, TicketUpdateDto updateDto, CancellationToken ct = default)
        {
            var exists = await _unitOfWork.Tickets.ExistsAsync(id, ct);
            if (!exists)
            {
                throw new NotFoundException(nameof(Ticket), id);
            }

            var ticket = _mapper.Map<Ticket>(updateDto);
            ticket.Id = id;

            await _unitOfWork.Tickets.UpdateAsync(ticket);

            return _mapper.Map<TicketReadDto>(ticket);
        }
    }
}
