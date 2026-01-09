using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport;
using AirportTool.Application.Options;
using AirportTool.Application.Services;
using AirportTool.Domain.Entities;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace AirportTool.Application.Tests.Services
{
    public class ScheduleImportServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<ILogger<ScheduleImportService>> logger = new();
        private readonly Mock<IValidator<ScheduleImportRowDto>> validator = new();

        private readonly Mock<IFlightRepository> flights = new();
        private readonly Mock<IFlightScheduleRepository> flightSchedules = new();
        private readonly Mock<IAircraftRepository> aircrafts = new();
        private readonly Mock<IAirlineRepository> airlines = new();
        private readonly Mock<IAirportRepository> airports = new();
        private readonly Mock<IGateRepository> gates = new();
        private readonly Mock<IFlightStatusRepository> flightStatuses = new();

        private readonly ScheduleImportService scheduleImportService;

        public ScheduleImportServiceTests()
        {
            unitOfWork.Setup(u => u.Flights).Returns(flights.Object);
            unitOfWork.Setup(u => u.FlightSchedules).Returns(flightSchedules.Object);
            unitOfWork.Setup(u => u.Aircrafts).Returns(aircrafts.Object);
            unitOfWork.Setup(u => u.Airlines).Returns(airlines.Object);
            unitOfWork.Setup(u => u.Airports).Returns(airports.Object);
            unitOfWork.Setup(u => u.Gates).Returns(gates.Object);
            unitOfWork.Setup(u => u.FlightStatuses).Returns(flightStatuses.Object);

            var options = Microsoft.Extensions.Options.Options.Create(
                        new ScheduleImportOptions { MaxRows = 1000 });


            scheduleImportService = new ScheduleImportService(
                unitOfWork.Object,
                options,
                logger.Object,
                validator.Object,
                bufferMinutes: 30);
        }

        private static MemoryStream ToStream(string json)
            => new MemoryStream(Encoding.UTF8.GetBytes(json));

        [Fact]
        public async Task ImportAsync_StreamIsNull_ThrowsBadRequestException()
        {
            // Arrange
            Stream? stream = null;

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                scheduleImportService.ImportAsync(stream!, CancellationToken.None));

            Assert.Contains("File stream is missing", ex.Message);
        }

        [Fact]
        public async Task ImportAsync_InvalidJson_ThrowsBadRequestException()
        {
            // Arrange
            using var stream = ToStream("{ this is not valid json }");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                scheduleImportService.ImportAsync(stream, CancellationToken.None));

            Assert.Contains("Invalid JSON file", ex.Message);
        }

        [Fact]
        public async Task ImportAsync_JsonRootNotArray_ThrowsBadRequestException()
        {
            // Arrange
            using var stream = ToStream(@"{ ""hello"": ""world"" }");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                scheduleImportService.ImportAsync(stream, CancellationToken.None));

            Assert.Contains("JSON must be an array", ex.Message);
        }

        [Fact]
        public async Task ImportAsync_JsonArrayExceedsMaxRows_ThrowsBadRequestException()
        {
            var options = Microsoft.Extensions.Options.Options.Create(new ScheduleImportOptions { MaxRows = 1 });
            var localSut = new ScheduleImportService(unitOfWork.Object, options, logger.Object, validator.Object);

            using var stream = ToStream(@"[
              { ""flightNumber"": ""RO391"", ""airlineIata"": ""RO"", ""originIata"": ""OTP"", ""destinationIata"": ""LHR"",
                ""scheduledDepartureUtc"": ""2025-12-01T06:30:00Z"", ""scheduledArrivalUtc"": ""2025-12-01T08:25:00Z"" },
              { ""flightNumber"": ""RO392"", ""airlineIata"": ""RO"", ""originIata"": ""OTP"", ""destinationIata"": ""LHR"",
                ""scheduledDepartureUtc"": ""2025-12-02T06:30:00Z"", ""scheduledArrivalUtc"": ""2025-12-02T08:25:00Z"" }
            ]");

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                localSut.ImportAsync(stream, CancellationToken.None));

            Assert.Contains("exceeds maximum allowed rows", ex.Message);
        }

        [Fact]
        public async Task ImportAsync_ValidRowWithNoExistingSchedule_ReturnsCreatedSummary()
        {
            // Arrange
            var ct = CancellationToken.None;

            validator.Setup(v => v.Validate(It.IsAny<ScheduleImportRowDto>()))
                .Returns(new ValidationResult()); 

            airlines.Setup(r => r.GetByIataCodeAsync("RO", ct))
                .ReturnsAsync(new Airline { Id = 1 });

            airports.Setup(r => r.GetByIataCodeAsync("OTP", ct))
                .ReturnsAsync(new Airport { Id = 10 });

            airports.Setup(r => r.GetByIataCodeAsync("LHR", ct))
                .ReturnsAsync(new Airport { Id = 20 });

            flights.Setup(r => r.GetByRouteAsync(1, "RO391", 10, 20, ct))
                .ReturnsAsync((Flight?)null);

            flights.Setup(r => r.AddAsync(It.IsAny<Flight>(), ct))
                .ReturnsAsync(new Flight
                {
                    Id = 100,
                    AirlineId = 1,
                    FlightNumber = "RO391",
                    OriginAirportId = 10,
                    DestinationAirportId = 20,
                    IsActive = true
                });

            gates.Setup(r => r.GetGateIdByCodeAndAirportAsync(10, "A12", ct))
                .ReturnsAsync(12);

            aircrafts.Setup(r => r.GetByTailNumberAsync("YR-BGA", ct))
                .ReturnsAsync(new Aircraft { Id = 5, TailNumber = "YR-BGA", SeatCapacity = 180 });

            var departureUtc = new DateTime(2025, 12, 01, 06, 30, 00, DateTimeKind.Utc);
            var arrivalUtc = new DateTime(2025, 12, 01, 08, 25, 00, DateTimeKind.Utc);

            flightSchedules.Setup(r => r.GetByFlightAndDepartureAsync(100, departureUtc, ct))
                .ReturnsAsync((FlightSchedule?)null);

            flightSchedules.Setup(r => r.HasGateOverlapAsync(12, departureUtc, 30, null, ct))
                .ReturnsAsync(false);

            flightStatuses.Setup(r => r.GetIdByStatusAsync("Planned", ct))
                .ReturnsAsync(1);

            flightSchedules.Setup(r => r.AddAsync(It.IsAny<FlightSchedule>(), ct))
                .ReturnsAsync(new FlightSchedule { Id = 777, FlightId = 100 });

            using var stream = ToStream(@"[
              {
                ""flightNumber"": ""RO391"",
                ""airlineIata"": ""ro"",
                ""originIata"": ""otp"",
                ""destinationIata"": ""lhr"",
                ""scheduledDepartureUtc"": ""2025-12-01T06:30:00Z"",
                ""scheduledArrivalUtc"": ""2025-12-01T08:25:00Z"",
                ""gateCode"": ""A12"",
                ""assignedAircraftTail"": ""YR-BGA""
              }
            ]");

            // Act
            var summary = await scheduleImportService.ImportAsync(stream, ct);

            // Assert
            Assert.Equal(1, summary.Total);
            Assert.Equal(1, summary.Created);
            Assert.Equal(0, summary.Updated);
            Assert.Empty(summary.Errors);

            Assert.Single(summary.Results);
            Assert.Equal("Created", summary.Results[0].Status);
            Assert.Equal(100, summary.Results[0].FlightId);
            Assert.Equal(777, summary.Results[0].FlightScheduleId);

            flightSchedules.Verify(r => r.AddAsync(It.IsAny<FlightSchedule>(), ct), Times.Once);
            flightSchedules.Verify(r => r.UpdateAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ImportAsync_GateOverlap_AddsErrorAndDoesNotCreateSchedule()
        {
            // Arrange
            var ct = CancellationToken.None;

            validator.Setup(v => v.Validate(It.IsAny<ScheduleImportRowDto>()))
                .Returns(new ValidationResult());

            airlines.Setup(r => r.GetByIataCodeAsync("RO", ct))
                .ReturnsAsync(new Airline { Id = 1 });

            airports.Setup(r => r.GetByIataCodeAsync("OTP", ct))
                .ReturnsAsync(new Airport { Id = 10 });

            airports.Setup(r => r.GetByIataCodeAsync("LHR", ct))
                .ReturnsAsync(new Airport { Id = 20 });

            flights.Setup(r => r.GetByRouteAsync(1, "RO391", 10, 20, ct))
                .ReturnsAsync(new Flight { Id = 100, AirlineId = 1, OriginAirportId = 10, DestinationAirportId = 20, FlightNumber = "RO391", IsActive = true });

            gates.Setup(r => r.GetGateIdByCodeAndAirportAsync(10, "A12", ct))
                .ReturnsAsync(12);

            var departureUtc = new DateTime(2025, 12, 01, 06, 30, 00, DateTimeKind.Utc);
            flightSchedules.Setup(r => r.GetByFlightAndDepartureAsync(100, departureUtc, ct))
                .ReturnsAsync((FlightSchedule?)null);

            flightSchedules.Setup(r => r.HasGateOverlapAsync(12, departureUtc, 30, null, ct))
                .ReturnsAsync(true);

            using var stream = ToStream(@"[
              {
                ""flightNumber"": ""RO391"",
                ""airlineIata"": ""RO"",
                ""originIata"": ""OTP"",
                ""destinationIata"": ""LHR"",
                ""scheduledDepartureUtc"": ""2025-12-01T06:30:00Z"",
                ""scheduledArrivalUtc"": ""2025-12-01T08:25:00Z"",
                ""gateCode"": ""A12""
              }
            ]");

            // Act
            var summary = await scheduleImportService.ImportAsync(stream, ct);

            // Assert
            Assert.Equal(1, summary.Total);
            Assert.Equal(0, summary.Created);
            Assert.Equal(0, summary.Updated);
            Assert.Single(summary.Errors);

            Assert.Contains("Gate overlap", summary.Errors[0].Message);
            Assert.Contains("OTP:A12", summary.Errors[0].Message);

            flightSchedules.Verify(r => r.AddAsync(It.IsAny<FlightSchedule>(), ct), Times.Never);
            flightSchedules.Verify(r => r.UpdateAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
