using AirportTool.Domain.Entities;
using System;

namespace AirportTool.Application.Contracts.Repositories
{
    public interface IFlightScheduleRepository : IGenericRepository<FlightSchedule>
    {
        Task<IEnumerable<FlightScheduleBasicInfo>> FindByRouteAndDateAsync(
            string originIata,
            string destinationIata,
            DateTime departureDate,
            CancellationToken ct = default);

        Task<IEnumerable<UpcomingFlights>> UpcomingFlights(int days, CancellationToken ct);

        Task<bool> HasGateOverlapAsync(
                int gateId,
                DateTime scheduledDepartureUtc,
                int bufferMinutes,
                CancellationToken ct = default);

        Task<bool> HasGateOverlapAsync(
            int gateId,
            DateTime scheduledDepartureUtc,
            int bufferMinutes,
            int? excludeFlightScheduleId,
            CancellationToken ct = default);

        Task<FlightSchedule?> GetByFlightAndDepartureAsync(
            int flightId,
            DateTime scheduledDepartureUtc,
            CancellationToken ct = default);
    }
}
