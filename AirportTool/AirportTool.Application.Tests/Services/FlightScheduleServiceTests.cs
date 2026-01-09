using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.Application.ModelDto.FlightSchedule.Stats;
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

namespace AirportTool.Application.Tests.Services
{
    public class FlightScheduleServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IMapper> mapper = new();
        private readonly Mock<IFlightScheduleRepository> flightScheduleRepo = new();

        private readonly FlightScheduleService flightScheduleService;

        public FlightScheduleServiceTests()
        {
            unitOfWork.Setup(u => u.FlightSchedules).Returns(flightScheduleRepo.Object);

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
        [Fact]
        public async Task CreateAsync_ValidInput_AddsScheduleAndReturnsDto()
        {
            // Arrange
            var ct = CancellationToken.None;

            var dto = new FlightScheduleCreateDto
            {
                FlightId = 101,
                GateId = 12,
                ScheduledDepartureUtc = new DateTime(2025, 12, 01, 11, 00, 00, DateTimeKind.Utc),
                ScheduledArrivalUtc = new DateTime(2025, 12, 01, 13, 00, 00, DateTimeKind.Utc),
                AssignedAircraftId = 5,
                StatusId = 1
            };

            var mappedSchedule = new FlightSchedule
            {
                FlightId = dto.FlightId,
                GateId = dto.GateId,
                ScheduledDepartureUtc = dto.ScheduledDepartureUtc,
                ScheduledArrivalUtc = dto.ScheduledArrivalUtc
            };

            var createdSchedule = new FlightSchedule
            {
                Id = 77,
                FlightId = dto.FlightId,
                GateId = dto.GateId,
                ScheduledDepartureUtc = dto.ScheduledDepartureUtc,
                ScheduledArrivalUtc = dto.ScheduledArrivalUtc
            };

            var expectedDto = new FlightScheduleReadDto { Id = 77 };

            mapper.Setup(m => m.Map<FlightSchedule>(dto)).Returns(mappedSchedule);

            flightScheduleRepo.Setup(r => r.HasGateOverlapAsync(dto.GateId.Value, dto.ScheduledDepartureUtc, 60, ct))
                .ReturnsAsync(false);

            flightScheduleRepo.Setup(r => r.AddAsync(mappedSchedule, ct))
                .ReturnsAsync(createdSchedule);

            mapper.Setup(m => m.Map<FlightScheduleReadDto>(createdSchedule))
                .Returns(expectedDto);

            // Act
            var result = await flightScheduleService.CreateAsync(dto, ct);

            // Assert
            Assert.Equal(77, result.Id);

            flightScheduleRepo.Verify(r => r.HasGateOverlapAsync(dto.GateId.Value, dto.ScheduledDepartureUtc, 60, ct), Times.Once);
            flightScheduleRepo.Verify(r => r.AddAsync(mappedSchedule, ct), Times.Once);
            mapper.Verify(m => m.Map<FlightScheduleReadDto>(createdSchedule), Times.Once);
        }
        
        [Fact]
        public async Task GetByIdAsync_ScheduleExists_ReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 5;

            var schedule = new FlightSchedule { Id = id, FlightId = 101 };
            var dto = new FlightScheduleReadDto { Id = id, FlightId = 101 };

            flightScheduleRepo.Setup(r => r.GetByIdAsync(id, ct)).ReturnsAsync(schedule);
            mapper.Setup(m => m.Map<FlightScheduleReadDto>(schedule)).Returns(dto);

            // Act
            var result = await flightScheduleService.GetByIdAsync(id, ct);

            // Assert
            Assert.Equal(id, result.Id);
            Assert.Equal(101, result.FlightId);
        }
        [Fact]
        public async Task GetByIdAsync_ScheduleDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 999;

            flightScheduleRepo.Setup(r => r.GetByIdAsync(id, ct)).ReturnsAsync((FlightSchedule?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => flightScheduleService.GetByIdAsync(id, ct));

            flightScheduleRepo.Verify(r => r.GetByIdAsync(id, ct), Times.Once);
            mapper.Verify(m => m.Map<FlightScheduleReadDto>(It.IsAny<FlightSchedule>()), Times.Never);
        }

        [Fact]
        public async Task FindByRouteAndDateAsync_OriginIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var ct = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                flightScheduleService.FindByRouteAndDateAsync("", "LHR", DateTime.UtcNow, ct));

            flightScheduleRepo.Verify(r => r.FindByRouteAndDateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task FindByRouteAndDateAsync_DestinationIsEmpty_ThrowsBadRequestException()
        {
            // Arrange
            var ct = CancellationToken.None;

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(() =>
                flightScheduleService.FindByRouteAndDateAsync("OTP", "   ", DateTime.UtcNow, ct));

            flightScheduleRepo.Verify(r => r.FindByRouteAndDateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTime>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetFlightStats_ValidInput_CallsRepoAndReturnsMappedDtos()
        {
            // Arrange
            var ct = CancellationToken.None;
            var days = 7;

            var upcoming = new List<UpcomingFlights>
            {
                new UpcomingFlights { Date = new DateTime(2025, 12, 01), Count = 3 },
                new UpcomingFlights { Date = new DateTime(2025, 12, 02), Count = 5 }
            };

            var expectedDtos = new List<UpcomingFlightsDto>
            {
                new UpcomingFlightsDto { Date = new DateTime(2025, 12, 01), Count = 3 },
                new UpcomingFlightsDto { Date = new DateTime(2025, 12, 02), Count = 5 }
            };

            flightScheduleRepo.Setup(r => r.UpcomingFlights(days, ct))
                .ReturnsAsync(upcoming);

            mapper.Setup(m => m.Map<IEnumerable<UpcomingFlightsDto>>(upcoming))
                .Returns(expectedDtos);

            // Act
            var result = await flightScheduleService.GetFlightStats(days, ct);

            // Assert
            Assert.Same(expectedDtos, result);

            flightScheduleRepo.Verify(r => r.UpcomingFlights(days, ct), Times.Once);
            mapper.Verify(m => m.Map<IEnumerable<UpcomingFlightsDto>>(upcoming), Times.Once);
        }
    }
}
