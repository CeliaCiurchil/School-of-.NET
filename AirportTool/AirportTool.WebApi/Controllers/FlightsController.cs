using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.Flight;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AirportTool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        public readonly IUnitOfWork _unitOfWork;

        public FlightsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Flights/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FlightReadDto>> GetById(int id, CancellationToken ct)
        {
            var flight = await _unitOfWork.Flights.GetByIdAsync(id, ct);
            if (flight is null)
                return NotFound();

            return Ok(flight);
        }
    }
}
