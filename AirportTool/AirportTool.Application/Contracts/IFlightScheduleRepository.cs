using AirportTool.Domain.Entities;
using System;

namespace AirportTool.Application.Contracts
{
    public interface IFlightScheduleRepository : IGenericRepository<FlightSchedule>
    {
        Task<IEnumerable<FlightScheduleBasicInfo>> FindByRouteAndDateAsync(
            string originIata,
            string destinationIata,
            DateTime departureDate,
            CancellationToken ct = default);

        Task<IEnumerable<UpcomingFlights>> UpcomingFlights(int days,CancellationToken ct);
    }
}
