using AirportTool.Application.Contracts;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport;
using AirportTool.Application.Options;
using AirportTool.WebApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AirportTool.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly IFlightScheduleService _flightScheduleService;
        private readonly IScheduleImportService _scheduleImportService;
        private readonly ScheduleImportOptions _importOptions;

        public SchedulesController(
            IFlightScheduleService flightScheduleService,
            IScheduleImportService scheduleImportService,
            IOptions<ScheduleImportOptions> importOptions)
        {
            _flightScheduleService = flightScheduleService;
            _scheduleImportService = scheduleImportService;
            _importOptions = importOptions.Value;
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

        [HttpPost("import")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<ScheduleImportSummaryDto>> ImportFlightSchedules(
            [FromForm] FileImportRequest? fileReq,
            CancellationToken ct)
        {
            var file = fileReq?.File;
            if (file is null || file.Length == 0)
            {
                return Problem("File is missing or empty.", statusCode: StatusCodes.Status400BadRequest);
            }

            if (file.Length > _importOptions.MaxFileSizeBytes)
            {
                return Problem("File size exceeds the allowed limit.", statusCode: StatusCodes.Status400BadRequest);
            }

            var extension = Path.GetExtension(file.FileName);
            if (!string.Equals(extension, ".json", StringComparison.OrdinalIgnoreCase))
            {
                return Problem("File extension must be .json.", statusCode: StatusCodes.Status400BadRequest);
            }

            try
            {
                await using var stream = file.OpenReadStream();
                var result = await _scheduleImportService.ImportAsync(stream, ct);
                var statusCode = GetImportStatusCode(result);
                return StatusCode(statusCode, result);
            }
            catch (BadRequestException ex)
            {
                return Problem(ex.Message, statusCode: StatusCodes.Status400BadRequest);
            }
        }

        [HttpGet("stats/upcoming/{days}")]
        public async Task<ActionResult<IEnumerable<UpcomingFlightsDto>>> GetFlightStats(int days, CancellationToken ct)
        {
            var stats = await _flightScheduleService.GetFlightStats(days, ct);
            return Ok(stats);
        }

        private static int GetImportStatusCode(ScheduleImportSummaryDto summary)
        {
            if (summary.Errors.Count > 0)
            {
                return StatusCodes.Status207MultiStatus;
            }

            if (summary.Created == summary.Total && summary.Updated == 0)
            {
                return StatusCodes.Status201Created;
            }

            return StatusCodes.Status200OK;
        }
    }
}
