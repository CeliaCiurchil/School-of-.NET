using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.Flight;
using AirportTool.Application.ModelDto.FlightSchedule;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _flightService;
        private readonly IFlightScheduleService _flightScheduleService;

        public FlightsController(IFlightService flightService, IFlightScheduleService flightScheduleService)
        {
            _flightService = flightService;
            _flightScheduleService = flightScheduleService;
        }

        // GET: api/Flights
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<FlightReadDto>>> GetAll(CancellationToken ct)
        {
            var flights = await _flightService.GetAllAsync(ct);
            return Ok(flights);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FlightScheduleBasicInfoDto>>> GetFlightInfos(
            [FromQuery(Name = "origin")] string? originAirport,
            [FromQuery(Name = "destination")] string? destinationAirport,
            [FromQuery(Name = "date")] DateTime? departureDate,
            CancellationToken ct = default)
        {

            if (departureDate is null)
            {
                return BadRequest("date query parameter is required.");
            }

            var schedules = await _flightScheduleService.FindByRouteAndDateAsync(
                originAirport,
                destinationAirport,
                departureDate.Value,
                ct);

            return Ok(schedules);
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightReadDto>> GetById(int id, CancellationToken ct)
        {
            var flight = await _flightService.GetByIdAsync(id, ct);
            return Ok(flight);
        }

        [HttpPost]
        public async Task<ActionResult<FlightReadDto>> Create(FlightCreateDto dto, CancellationToken ct)
        {
            var createdFlight = await _flightService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = createdFlight.Id }, createdFlight);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<FlightReadDto>> Update(int id, FlightUpdateDto dto, CancellationToken ct)
        {
            var updated = await _flightService.UpdateAsync(id, dto, ct);

            return Ok(updated);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var deleted = await _flightService.DeleteAsync(id, ct);

            return NoContent();
        }
    }
}
