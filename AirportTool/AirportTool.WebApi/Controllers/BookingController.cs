using System.Security.Claims;
using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.Booking;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<ActionResult<BookingReadDto>> Create(CancellationToken ct)
        {
            var userId = User.FindFirstValue("uid");
            if (string.IsNullOrWhiteSpace(userId))
            {
                return Unauthorized();
            }

            var createDto = new BookingCreateDto() { UserId = userId };
            var booking = await _bookingService.CreateAsync(createDto, ct);
            return CreatedAtAction(nameof(GetByConfirmationCode), new { confirmationCode = booking.ConfirmationCode }, booking);
        }

        [Authorize(Roles = "Client")]
        [HttpGet("{confirmationCode}")]
        public async Task<ActionResult<BookingReadDto>> GetByConfirmationCode(string confirmationCode, CancellationToken ct)
        {
            var booking = await _bookingService.GetByConfirmationAsync(confirmationCode, ct);
            return Ok(booking);
        }

        [Authorize(Roles = "Client")]
        [HttpDelete("{confirmationCode}")]    
        public async Task<IActionResult> Cancel(string confirmationCode, CancellationToken ct)
        {
            await _bookingService.CancelBookingAsync(confirmationCode, ct);
            return NoContent();
        }
    }
}
