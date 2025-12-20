using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.FlightSchedule;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IFlightScheduleService _flightScheduleService;

        public SchedulesController(IFlightScheduleService flightScheduleService)
        {
            _flightScheduleService = flightScheduleService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<FlightScheduleReadDto>> GetById(int id, CancellationToken ct)
        {
            var schedule = await _flightScheduleService.GetByIdAsync(id, ct);
            return Ok(schedule);
        }

        [HttpPost]
        public async Task<ActionResult<FlightScheduleReadDto>> Create(FlightScheduleCreateDto dto, CancellationToken ct)
        {
            var created = await _flightScheduleService.CreateAsync(dto, ct);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpGet("stats/upcoming/{days}")]
        public async Task<ActionResult<IEnumerable<UpcomingFlightsDto>>> GetFlightStats(int days, CancellationToken ct)
        {
            var stats = await _flightScheduleService.GetFlightStats(days, ct);
            return Ok(stats);
        }
    }
}
