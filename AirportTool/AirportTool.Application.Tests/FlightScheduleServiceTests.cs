using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.Application.Services;
using AirportTool.Domain.Entities;
using AutoMapper;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.Application.Tests
{
    public class FlightScheduleServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IMapper> mapper = new();
        private readonly Mock<IFlightScheduleRepository> flightScheduleRepo = new();

        private readonly FlightScheduleService flightScheduleService;

        public FlightScheduleServiceTests()
        {
            flightScheduleService = new FlightScheduleService(
                unitOfWork.Object,
                mapper.Object,
                bufferMinutes: 60);
        }

        [Fact]
        public async Task CreateAsync_GateOverlapDetected_ThrowsConflictException()
        {
            // Arrange
            var ct = CancellationToken.None;

            unitOfWork.Setup(u => u.FlightSchedules).Returns(flightScheduleRepo.Object);

            var dto = new FlightScheduleCreateDto
            {
                FlightId = 101,
                GateId = 12,
                ScheduledDepartureUtc = new DateTime(2025, 12, 01, 11, 00, 00, DateTimeKind.Utc),
                ScheduledArrivalUtc = new DateTime(2025, 12, 01, 13, 00, 00, DateTimeKind.Utc),
                AssignedAircraftId = 5,
                StatusId = 1
            };

            mapper.Setup(m => m.Map<FlightSchedule>(dto))
                .Returns(new FlightSchedule
                {
                    FlightId = dto.FlightId,
                    GateId = dto.GateId,
                    ScheduledDepartureUtc = dto.ScheduledDepartureUtc,
                    ScheduledArrivalUtc = dto.ScheduledArrivalUtc
                });

            flightScheduleRepo.Setup(r => r.HasGateOverlapAsync(dto.GateId.Value, dto.ScheduledDepartureUtc, 60, ct))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ConflictException>(() => flightScheduleService.CreateAsync(dto, ct));

            Assert.Contains("Gate overlap detected", ex.Message);
            Assert.Contains($"Gate {dto.GateId}", ex.Message);
            Assert.Contains("2025-12-01 10:00", ex.Message);
            Assert.Contains("2025-12-01 12:00", ex.Message);

            flightScheduleRepo.Verify(
                r => r.HasGateOverlapAsync(dto.GateId.Value, dto.ScheduledDepartureUtc, 60, ct),
                Times.Once);

            flightScheduleRepo.Verify(
                r => r.AddAsync(It.IsAny<FlightSchedule>(), It.IsAny<CancellationToken>()),
                Times.Never);
        }
    }
}

