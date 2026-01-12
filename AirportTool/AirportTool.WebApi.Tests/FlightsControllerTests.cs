using AirportTool.Application.Contracts.Services;
using AirportTool.Application.ModelDto.Flight;
using AirportTool.Application.ModelDto.FlightSchedule;
using AirportTool.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.WebApi.Tests
{
    public class FlightsControllerTests
    {
        private readonly Mock<IFlightService> flightService = new();
        private readonly Mock<IFlightScheduleService> flightScheduleService = new();

        private readonly FlightsController controller;

        public FlightsControllerTests()
        {
            controller = new FlightsController(
                flightService.Object,
                flightScheduleService.Object);
        }

        [Fact]
        public async Task GetAll_ServiceReturnsFlights_ReturnsOkWithFlights()
        {
            // Arrange
            var expectedFlights = new List<FlightReadDto>
            {
                new FlightReadDto { Id = 1, AirlineId = 1, FlightNumber = "DL200", OriginAirportId = 10, DestinationAirportId = 20, DefaultAircraftId = 5, IsActive = true },
                new FlightReadDto { Id = 2, AirlineId = 2, FlightNumber = "AA150", OriginAirportId = 15, DestinationAirportId = 25, DefaultAircraftId = 6, IsActive = true }
            };
            flightService
                .Setup(s => s.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFlights);

            // Act
            var actionResult = await controller.GetAll(CancellationToken.None);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            Assert.Same(expectedFlights, ok.Value);
            flightService.Verify(s => s.GetAllAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task GetFlightInfos_DateIsNull_ReturnsBadRequest()
        {
            // Arrange
            var ct = CancellationToken.None;

            // Act
            var actionResult = await controller.GetFlightInfos("OTP", "LHR", null, ct);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequest.StatusCode);
            Assert.Equal("date query parameter is required.", badRequest.Value);
            flightScheduleService.Verify(
                s => s.FindByRouteAndDateAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()),
                Times.Never);
        }

        [Fact]
        public async Task GetFlightInfos_ValidQuery_CallsServiceAndReturnsOkWithSchedules()
        {
            // Arrange
            var ct = CancellationToken.None;
            var origin = "OTP";
            var destination = "LHR";
            var date = new DateTime(2025, 12, 01, 00, 00, 00, DateTimeKind.Utc);

            var expected = new List<FlightScheduleBasicInfoDto>
            {
                new FlightScheduleBasicInfoDto
                {
                    ScheduleId = 1,
                    FlightId = 101,
                    FlightNumber = "RO391",
                    AirlineCode = "RO",
                    OriginAirportCode = origin,
                    DestinationAirportCode = destination,
                    ScheduledDepartureUtc = date.AddHours(11),
                    ScheduledArrivalUtc = date.AddHours(13)
                }
            };

            flightScheduleService
                .Setup(s => s.FindByRouteAndDateAsync(origin, destination, date, ct))
                .ReturnsAsync(expected);

            // Act
            var actionResult = await controller.GetFlightInfos(origin, destination, date, ct);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            Assert.Same(expected, ok.Value);
            flightScheduleService.Verify(s => s.FindByRouteAndDateAsync(origin, destination, date, ct), Times.Once);
        }

        [Fact]
        public async Task GetById_ServiceReturnsFlight_ReturnsOkWithFlight()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 42;
            var expected = new FlightReadDto
            {
                Id = id,
                AirlineId = 1,
                FlightNumber = "RO391",
                OriginAirportId = 10,
                DestinationAirportId = 20,
                DefaultAircraftId = 5,
                IsActive = true
            };

            flightService.Setup(s => s.GetByIdAsync(id, ct)).ReturnsAsync(expected);

            // Act
            var actionResult = await controller.GetById(id, ct);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            Assert.Same(expected, ok.Value);
            flightService.Verify(s => s.GetByIdAsync(id, ct), Times.Once);
        }

        [Fact]
        public async Task Create_ValidDto_ReturnsCreatedAtActionWithCreatedFlight()
        {
            // Arrange
            var ct = CancellationToken.None;
            var dto = new FlightCreateDto
            {
                AirlineIata = "RO",
                FlightNumber = "RO391",
                OriginIata = "OTP",
                DestinationIata = "LHR",
                DefaultAircraftTail = "YR-BGA",
                IsActive = true
            };
            var created = new FlightReadDto
            {
                Id = 100,
                AirlineId = 1,
                FlightNumber = dto.FlightNumber,
                OriginAirportId = 10,
                DestinationAirportId = 20,
                DefaultAircraftId = 5,
                IsActive = dto.IsActive
            };

            flightService.Setup(s => s.CreateAsync(dto, ct)).ReturnsAsync(created);

            // Act
            var actionResult = await controller.Create(dto, ct);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(nameof(FlightsController.GetById), createdAt.ActionName);
            Assert.Equal(created.Id, createdAt.RouteValues?["id"]);
            Assert.Same(created, createdAt.Value);
            flightService.Verify(s => s.CreateAsync(dto, ct), Times.Once);
        }

        [Fact]
        public async Task Update_ValidDto_ReturnsOkWithUpdatedFlight()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 123;
            var dto = new FlightUpdateDto
            {
                AirlineIata = "RO",
                FlightNumber = "RO392",
                OriginIata = "OTP",
                DestinationIata = "AMS",
                DefaultAircraftTail = "YR-BGB",
                IsActive = true
            };
            var updated = new FlightReadDto
            {
                Id = id,
                AirlineId = 1,
                FlightNumber = dto.FlightNumber,
                OriginAirportId = 10,
                DestinationAirportId = 30,
                DefaultAircraftId = 6,
                IsActive = dto.IsActive
            };

            flightService.Setup(s => s.UpdateAsync(id, dto, ct)).ReturnsAsync(updated);

            // Act
            var actionResult = await controller.Update(id, dto, ct);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            Assert.Same(updated, ok.Value);
            flightService.Verify(s => s.UpdateAsync(id, dto, ct), Times.Once);
        }

        [Fact]
        public async Task Delete_ValidId_ReturnsNoContent()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 99;
            flightService.Setup(s => s.DeleteAsync(id, ct)).ReturnsAsync(true);

            // Act
            var result = await controller.Delete(id, ct);

            // Assert
            Assert.IsType<NoContentResult>(result);
            flightService.Verify(s => s.DeleteAsync(id, ct), Times.Once);
        }
    }
}