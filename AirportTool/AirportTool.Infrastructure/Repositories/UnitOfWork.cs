using AirportTool.Application.Contracts;
using AirportTool.Infrastructure.Persistence;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FlightBookingDbContext _context;

        public IFlightRepository Flights { get; }

        public UnitOfWork(FlightBookingDbContext context, IMapper mapper, IFlightRepository flightRepository)
        {
            _context = context;
            Flights = flightRepository;
        }

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

