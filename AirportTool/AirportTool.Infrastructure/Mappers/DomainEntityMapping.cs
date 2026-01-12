using AirportTool.Domain.Entities;
using AirportTool.Infrastructure.Persistence.Identity;
using AutoMapper;
using HotelListing.API.Models.Users;
using Address = AirportTool.Domain.Entities.Address;
using AddressDb = AirportTool.Infrastructure.Persistence.Entities.Address;
using Aircraft = AirportTool.Domain.Entities.Aircraft;
using AircraftDb = AirportTool.Infrastructure.Persistence.Entities.Aircraft;
using Airline = AirportTool.Domain.Entities.Airline;
using AirlineDb = AirportTool.Infrastructure.Persistence.Entities.Airline;
using Airport = AirportTool.Domain.Entities.Airport;
using AirportDb = AirportTool.Infrastructure.Persistence.Entities.Airport;
using Booking = AirportTool.Domain.Entities.Booking;
using BookingDb = AirportTool.Infrastructure.Persistence.Entities.Booking;
using Flight = AirportTool.Domain.Entities.Flight;
using FlightDb = AirportTool.Infrastructure.Persistence.Entities.Flight;
using FlightSchedule = AirportTool.Domain.Entities.FlightSchedule;
using FlightScheduleDb = AirportTool.Infrastructure.Persistence.Entities.FlightSchedule;
using FlightStatus = AirportTool.Domain.Entities.FlightStatus;
using FlightStatusDb = AirportTool.Infrastructure.Persistence.Entities.FlightStatus;
using Gate = AirportTool.Domain.Entities.Gate;
using GateDb = AirportTool.Infrastructure.Persistence.Entities.Gate;
using Ticket = AirportTool.Domain.Entities.Ticket;
using TicketDb = AirportTool.Infrastructure.Persistence.Entities.Ticket;

namespace AirportTool.Infrastructure.Mappers
{
    public class DomainEntityMapping : Profile
    {
        public DomainEntityMapping()
        {
            CreateMap<ApiUser, ApiUserDto>().ReverseMap();
            CreateMap<Address, AddressDb>().ReverseMap();
            CreateMap<Flight, FlightDb>().ReverseMap();
            CreateMap<FlightScheduleDb, FlightSchedule>().ReverseMap();
            CreateMap<FlightStatusDb, FlightStatus>().ReverseMap();
            CreateMap<Gate, GateDb>().ReverseMap();
            CreateMap<Aircraft, AircraftDb>().ReverseMap();
            CreateMap<Airline, AirlineDb>().ReverseMap();
            CreateMap<Airport, AirportDb>().ReverseMap();
            CreateMap<Booking, BookingDb>().ReverseMap();
            CreateMap<Ticket, TicketDb>().ReverseMap();
            CreateMap<FlightScheduleDb, FlightScheduleBasicInfo>()
                .ForMember(dest => dest.ScheduleId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.FlightNumber, opt => opt.MapFrom(src => src.Flight.FlightNumber))
                .ForMember(dest => dest.AirlineCode, opt => opt.MapFrom(src => src.Flight.Airline.Iatacode))
                .ForMember(dest => dest.OriginAirportCode, opt => opt.MapFrom(src => src.Flight.OriginAirport.Iatacode))
                .ForMember(dest => dest.DestinationAirportCode, opt => opt.MapFrom(src => src.Flight.DestinationAirport.Iatacode));
        }
    }
}
