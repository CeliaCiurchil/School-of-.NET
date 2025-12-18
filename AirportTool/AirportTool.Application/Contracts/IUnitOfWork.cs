using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts
{
    public interface IUnitOfWork
    {
        public IFlightRepository Flights { get; }
        public Task<int> SaveChangesAsync();
    }
}
 