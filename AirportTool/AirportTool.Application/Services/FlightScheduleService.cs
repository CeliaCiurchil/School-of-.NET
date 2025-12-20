using AirportTool.Application.Contracts;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.Domain.Entities;
using AutoMapper;

namespace AirportTool.Application.Services
{
    public class FlightScheduleService : IFlightScheduleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightScheduleService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FlightScheduleReadDto> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var schedule = await _unitOfWork.FlightSchedules.GetByIdAsync(id, ct);
            
            var scheduleDto = schedule is not null
                ? _mapper.Map<FlightScheduleReadDto>(schedule)
                : null;
            return scheduleDto ?? throw new NotFoundException(typeof(FlightSchedule).Name, id);
        }

        public async Task<FlightScheduleReadDto> CreateAsync(FlightScheduleCreateDto dto, CancellationToken ct = default)
        {
            var schedule = _mapper.Map<FlightSchedule>(dto);
            var created = await _unitOfWork.FlightSchedules.AddAsync(schedule, ct);

            // Reload with related data for detailed response
            var scheduleDto = _mapper.Map<FlightScheduleReadDto>(created);

            return scheduleDto ?? throw new NotFoundException(typeof(FlightSchedule).Name, created.Id);
        }

        public async Task<IEnumerable<FlightScheduleBasicInfoDto>> FindByRouteAndDateAsync(
            string origin,
            string destination,
            DateTime departureDate,
            CancellationToken ct = default)
        {

            if (string.IsNullOrWhiteSpace(origin))
                throw new BadRequestException($"Origin airport code is required {nameof(origin)}");

            if (string.IsNullOrWhiteSpace(destination))
                throw new BadRequestException($"Destination airport code is required {nameof(destination)}");

            var schedules = await _unitOfWork.FlightSchedules.FindByRouteAndDateAsync(
                origin,
                destination,
                departureDate,
                ct);

            return _mapper.Map<IEnumerable<FlightScheduleBasicInfoDto>>(schedules);
        }

        public async Task<IEnumerable<UpcomingFlightsDto>> GetFlightStats(int days, CancellationToken ct = default)
        {
            IEnumerable<UpcomingFlights> upcomingFlights = await _unitOfWork.FlightSchedules.UpcomingFlights(days, ct);
            return _mapper.Map<IEnumerable<UpcomingFlightsDto>>(upcomingFlights);
        }
    }
}
