using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.Flight;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Services
{
    public class FlightService : IFlightService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FlightService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<FlightReadDto?> GetByIdAsync(int id, CancellationToken ct = default)
        {
            var flight = await _unitOfWork.Flights.GetByIdAsync(id, ct);
            return flight is null ? null : _mapper.Map<FlightReadDto>(flight);
        }
    }
}
