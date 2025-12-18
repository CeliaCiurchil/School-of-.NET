using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AirportTool.Application.ModelDto.Flight;

namespace AirportTool.Application.Contracts
{
    public interface IFlightService
    {
        public Task<FlightReadDto?> GetByIdAsync(int id, CancellationToken ct = default);
    }
}
