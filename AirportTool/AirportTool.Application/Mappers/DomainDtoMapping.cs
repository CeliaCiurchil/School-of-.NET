using AirportTool.Application.ModelDto.Aircraft;
using AirportTool.Application.ModelDto.Booking;
using AirportTool.Application.ModelDto.FlighStatus;
using AirportTool.Application.ModelDto.Flight;
using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.Application.ModelDto.FlightSchedule.Stats;
using AirportTool.Application.ModelDto.Gate;
using AirportTool.Application.ModelDto.Ticket;
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

            CreateMap<Booking, BookingReadDto>().ReverseMap();
            CreateMap<Ticket,TicketCreateDto>().ReverseMap();
            CreateMap<Ticket, TicketReadDto>().ReverseMap();
            CreateMap<Ticket, TicketUpdateDto>().ReverseMap();
        }
    }
}
