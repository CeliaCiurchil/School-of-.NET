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
            var withDetails = await _unitOfWork.FlightSchedules.GetByIdAsync(created.Id, ct);
            var scheduleDto = withDetails is not null
                ? _mapper.Map<FlightScheduleReadDto>(withDetails)
                : null;
            return scheduleDto ?? throw new NotFoundException(typeof(FlightSchedule).Name, created.Id);
        }
    }
}
