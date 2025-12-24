using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.Booking;
using Microsoft.AspNetCore.Http;
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
        public async Task<ActionResult<BookingReadDto>> Create(CancellationToken ct)
        {
            var createDto = new BookingCreateDto() { UserId = 1 };
            var booking = await _bookingService.CreateAsync(createDto, ct);
            return CreatedAtAction(nameof(GetByConfirmationCode), new { confirmationCode = booking.ConfirmationCode }, booking);
        }

        [HttpGet("{confirmationCode}")]
        public async Task<ActionResult<BookingReadDto>> GetByConfirmationCode(string confirmationCode, CancellationToken ct)
        {
            var booking = await _bookingService.GetByConfirmationAsync(confirmationCode, ct);
            return Ok(booking);
        }
        
        [HttpDelete("{confirmationCode}")]    
        public async Task<IActionResult> Cancel(string confirmationCode, CancellationToken ct)
        {
            await _bookingService.CancelBookingAsync(confirmationCode, ct);
            return NoContent();
        }
    }
}
