using AirportTool.Application.ModelDto.Flight;
using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.Domain.Entities;
using AutoMapper;

namespace AirportTool.Application.Mappers
{
    public class DomainDtoMapping : Profile
    {
        public DomainDtoMapping()
        {
            CreateMap<Flight, FlightReadDto>().ReverseMap();
            CreateMap<Flight, FlightCreateDto>().ReverseMap();
            CreateMap<Flight, FlightUpdateDto>().ReverseMap();

            CreateMap<FlightSchedule, FlightScheduleReadDto>().ReverseMap();
            CreateMap<FlightSchedule, FlightScheduleCreateDto>().ReverseMap();
            CreateMap<Aircraft, AircraftDto>().ReverseMap();
            CreateMap<Gate, GateDto>().ReverseMap();
            CreateMap<FlightStatus, FlightStatusDto>().ReverseMap();
            
            CreateMap<FlightScheduleBasicInfo, FlightScheduleBasicInfoDto>();
            CreateMap<UpcomingFlights, UpcomingFlightsDto>();
        }
    }
}
