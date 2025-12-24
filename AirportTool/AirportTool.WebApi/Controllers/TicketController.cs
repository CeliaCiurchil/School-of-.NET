using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.Ticket;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpPost]
        public async Task<ActionResult<TicketReadDto>> CreateTicket(TicketCreateDto createDto, CancellationToken ct)
        {
            var ticket = await _ticketService.CreateAsync(createDto, ct);
            return CreatedAtAction(nameof(GetTicketById), new { id = ticket.Id }, ticket);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TicketReadDto>> GetTicketById(int id, CancellationToken ct)
        {
            var ticket = await _ticketService.GetByIdAsync(id, ct);
            if (ticket == null)
            {
                return NotFound();
            }
            return Ok(ticket);
        }

        [HttpGet("by-flight/{flightId}")]
        public async Task<ActionResult<IEnumerable<TicketReadDto>>> GetTicketsByFlight(int flightId, CancellationToken ct)
        {
            var tickets = await _ticketService.GetTicketsByFlightIdAsync(flightId, ct);
            return Ok(tickets);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicket(int id, CancellationToken ct)
        {
            await _ticketService.DeleteAsync(id, ct);
            return NoContent();
        }
    }
}
