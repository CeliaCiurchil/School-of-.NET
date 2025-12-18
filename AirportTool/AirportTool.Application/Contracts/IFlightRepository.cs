using AirportTool.Application.ModelDto.Flight;
using AirportTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
    }
}
