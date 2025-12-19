using AutoMapper;
using Address = AirportTool.Domain.Entities.Address;
using AddressDb = AirportTool.Infrastructure.Persistence.Entities.Address;

using Flight = AirportTool.Domain.Entities.Flight;
using FlightDb = AirportTool.Infrastructure.Persistence.Entities.Flight;
using FlightSchedule = AirportTool.Domain.Entities.FlightSchedule;
using FlightScheduleDb = AirportTool.Infrastructure.Persistence.Entities.FlightSchedule;
using Gate = AirportTool.Domain.Entities.Gate;
using GateDb = AirportTool.Infrastructure.Persistence.Entities.Gate;
using Aircraft = AirportTool.Domain.Entities.Aircraft;
using AircraftDb = AirportTool.Infrastructure.Persistence.Entities.Aircraft;
using FlightStatus = AirportTool.Domain.Entities.FlightStatus;
using FlightStatusDb = AirportTool.Infrastructure.Persistence.Entities.FlightStatus;

namespace AirportTool.Infrastructure.Mappers
{
    public class DomainEntityMapping : Profile
    {
        public DomainEntityMapping()
        {
            CreateMap<Address, AddressDb>().ReverseMap();
            CreateMap<Flight, FlightDb>().ReverseMap();
            CreateMap<FlightSchedule, FlightScheduleDb>().ReverseMap();
            CreateMap<Gate, GateDb>().ReverseMap();
            CreateMap<Aircraft, AircraftDb>().ReverseMap();
            CreateMap<FlightStatus, FlightStatusDb>().ReverseMap();
        }
    }
}
