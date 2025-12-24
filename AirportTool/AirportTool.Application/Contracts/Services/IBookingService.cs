using AirportTool.Application.ModelDto.Booking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts.Services
{
    public interface IBookingService
    {
        Task<BookingReadDto?> GetByConfirmationAsync(string confirmationCode, CancellationToken ct);
        Task<BookingReadDto> CreateAsync(BookingCreateDto dto, CancellationToken ct = default);
        Task CancelBookingAsync(string confirmationCode, CancellationToken ct = default);
    }
}
