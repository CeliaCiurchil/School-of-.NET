using AirportTool.Domain.Entities;
using AutoMapper;
using Address = AirportTool.Domain.Entities.Address;
using AddressDb = AirportTool.Infrastructure.Persistence.Entities.Address;
using Aircraft = AirportTool.Domain.Entities.Aircraft;
using AircraftDb = AirportTool.Infrastructure.Persistence.Entities.Aircraft;
using Flight = AirportTool.Domain.Entities.Flight;
using FlightDb = AirportTool.Infrastructure.Persistence.Entities.Flight;
using FlightSchedule = AirportTool.Domain.Entities.FlightSchedule;
using FlightScheduleDb = AirportTool.Infrastructure.Persistence.Entities.FlightSchedule;
using FlightStatus = AirportTool.Domain.Entities.FlightStatus;
using FlightStatusDb = AirportTool.Infrastructure.Persistence.Entities.FlightStatus;
using Gate = AirportTool.Domain.Entities.Gate;
using GateDb = AirportTool.Infrastructure.Persistence.Entities.Gate;

namespace AirportTool.Infrastructure.Mappers
{
    public class DomainEntityMapping : Profile
    {
        public DomainEntityMapping()
        {
            CreateMap<Address, AddressDb>().ReverseMap();
            CreateMap<Flight, FlightDb>().ReverseMap();
            CreateMap<FlightScheduleDb, FlightSchedule>().ReverseMap();
            CreateMap<FlightStatusDb, FlightStatus>().ReverseMap();
            CreateMap<Gate, GateDb>().ReverseMap();
            CreateMap<Aircraft, AircraftDb>().ReverseMap();
            CreateMap<FlightScheduleDb, FlightScheduleBasicInfo>()
                .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Flight.FlightNumber))
                .ForMember(dest => dest.AirlineCode, opt => opt.MapFrom(src => src.Flight.Airline.Iatacode))
                .ForMember(dest => dest.OriginAirportCode, opt => opt.MapFrom(src => src.Flight.OriginAirport.Iatacode))
                .ForMember(dest => dest.DestinationAirportCode, opt => opt.MapFrom(src => src.Flight.DestinationAirport.Iatacode));
        }
    }
}
