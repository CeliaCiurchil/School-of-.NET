using AirportTool.Application.Contracts;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.Booking;
using AirportTool.Application.ModelDto.Flight;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Enums;
using AutoMapper;
using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task CancelBookingAsync(string confirmationCode, CancellationToken ct = default)
        {
            var exists = await _unitOfWork.Bookings.ExistsAsync(confirmationCode, ct);
            if (!exists)
            {
                throw new NotFoundException(nameof(Booking), confirmationCode);
            }
            await _unitOfWork.Bookings.CancelAsync(confirmationCode, ct);
        }

        public static string GenerateConfirmationCode()//move to an utils
            => Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();


        public async Task<BookingReadDto> CreateAsync(BookingCreateDto dto, CancellationToken ct = default)
        {
            //take user from loged in user
            var booking = new Booking
            {
                UserId = dto.UserId,
                BookingStatusId = (int)BookingStatus.Active,
                CreatedUtc = DateTime.UtcNow,
                ConfirmationCode = GenerateConfirmationCode(),
                Quantity = 0
            };

            var created = await _unitOfWork.Bookings.AddAsync(booking, ct);

            return _mapper.Map<BookingReadDto>(created);
        }

        public async Task<BookingReadDto?> GetByConfirmationAsync(string confirmationCode, CancellationToken ct)
        {
            var booking = await _unitOfWork.Bookings.GetByCodeAsync(confirmationCode, ct);
            var bookingdto = booking is not null
                ? _mapper.Map<BookingReadDto>(booking)
                : null;
            return bookingdto ?? throw new NotFoundException(typeof(Booking).Name, confirmationCode);
        }
    }
}
