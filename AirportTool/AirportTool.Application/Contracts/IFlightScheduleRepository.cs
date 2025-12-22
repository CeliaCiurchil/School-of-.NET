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

        Task<bool> HasGateOverlapAsync(
                int gateId,
                DateTime scheduledDepartureUtc,
                int bufferMinutes,
                CancellationToken ct = default);
    }
}
