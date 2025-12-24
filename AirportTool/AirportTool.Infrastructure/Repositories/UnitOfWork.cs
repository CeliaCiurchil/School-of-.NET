using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Infrastructure.Persistence;

namespace AirportTool.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly FlightBookingDbContext _context;

        public IFlightRepository Flights { get; }
        public IFlightScheduleRepository FlightSchedules { get; }
        public IAircraftRepository Aircrafts { get; }
        public IAirlineRepository Airlines { get; }
        public IAirportRepository Airports { get; }
        public IGateRepository Gates { get; }
        public IFlightStatusRepository FlightStatuses { get; }
        public IBookingRepository Bookings { get; }

        public ITicketRepository Tickets { get; }

        public UnitOfWork(
            FlightBookingDbContext context,
            IFlightRepository flightRepository,
            IFlightScheduleRepository flightScheduleRepository,
            IAircraftRepository aircraftRepository,
            IAirlineRepository airlineRepository,
            IAirportRepository airportRepository,
            IGateRepository gateRepository,
            IFlightStatusRepository flightStatusRepository,
            IBookingRepository bookingRepository,
            ITicketRepository ticketRepository)
        {
            _context = context;
            Flights = flightRepository;
            FlightSchedules = flightScheduleRepository;
            Aircrafts = aircraftRepository;
            Airlines = airlineRepository;
            Airports = airportRepository;
            Gates = gateRepository;
            FlightStatuses = flightStatusRepository;
            Bookings = bookingRepository;
            Tickets = ticketRepository;
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
