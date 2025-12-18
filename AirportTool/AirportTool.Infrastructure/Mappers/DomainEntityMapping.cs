using AutoMapper;
using Address = AirportTool.Domain.Entities.Address;
using AddressDb = AirportTool.Infrastructure.Persistence.Entities.Address;

using Flight = AirportTool.Domain.Entities.Flight;
using FlightDb = AirportTool.Infrastructure.Persistence.Entities.Flight;

namespace AirportTool.Infrastructure.Mappers
{
    public class DomainEntityMapping : Profile
    {
        public DomainEntityMapping()
        {
            CreateMap<Address, AddressDb>().ReverseMap();
            CreateMap<Flight, FlightDb>().ReverseMap();
        }
    }
}
