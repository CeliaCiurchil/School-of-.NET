using AirportTool.Application.ModelDto.Flight;
using AirportTool.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Mappers
{
    public class DomainDtoMapping : Profile
    {
        public DomainDtoMapping()
        {
            CreateMap<Flight,FlightReadDto>().ReverseMap();
            CreateMap<Flight,FlightCreateDto>().ReverseMap();
            CreateMap<Flight,FlightUpdateDto>().ReverseMap();

        }
    }
}
