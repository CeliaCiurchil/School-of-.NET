using AirportTool.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts
{
    public interface IAirlineRepository : IGenericRepository<Airline>
    {
        Task<Airline> GetByIataCodeAsync(string iataCode);
    }
}
