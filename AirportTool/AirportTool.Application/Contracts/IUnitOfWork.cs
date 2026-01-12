using AirportTool.Application.Contracts.Repositories;
using System.Diagnostics.Contracts;

namespace AirportTool.Application.Contracts
{
    public interface IUnitOfWork
    {
        public IFlightRepository Flights { get; }
        public IFlightScheduleRepository FlightSchedules { get; }
        public IAircraftRepository Aircrafts { get; }
        public IAirlineRepository Airlines { get; }
        public IAirportRepository Airports { get; }
        public IGateRepository Gates { get; }
        public IFlightStatusRepository FlightStatuses { get; }
        public IBookingRepository Bookings { get; }
        public ITicketRepository Tickets { get; }
        public Task<int> SaveChangesAsync();
    }
}
