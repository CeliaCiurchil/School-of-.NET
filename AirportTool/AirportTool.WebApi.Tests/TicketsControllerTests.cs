using AirportTool.Application.Contracts.Services;
using AirportTool.Application.ModelDto.Ticket;
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
    public class TicketControllerTests
    {
        private readonly Mock<ITicketService> ticketService = new();
        private readonly TicketController controller;

        public TicketControllerTests()
        {
            controller = new TicketController(ticketService.Object);
        }

        [Fact]
        public async Task CreateTicket_ValidDto_ReturnsCreatedAtActionWithTicket()
        {
            // Arrange
            var ct = CancellationToken.None;
            var create = new TicketCreateDto
            {
                BookingId = 1,
                FlightScheduleId = 10,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com"
            };
            var created = new TicketReadDto
            {
                Id = 100,
                BookingId = create.BookingId,
                FlightScheduleId = create.FlightScheduleId,
                FareClass = create.FareClass,
                SeatNumber = create.SeatNumber,
                PassengerFullName = create.PassengerFullName,
                PassengerEmail = create.PassengerEmail,
                BasePrice = 100,
                Taxes = 20,
                TotalPrice = 120,
                Currency = "USD",
                IsRefundable = true
            };
            ticketService.Setup(s => s.CreateAsync(create, ct)).ReturnsAsync(created);

            // Act
            var actionResult = await controller.CreateTicket(create, ct);

            // Assert
            var createdAt = Assert.IsType<CreatedAtActionResult>(actionResult.Result);
            Assert.Equal(nameof(TicketController.GetTicketById), createdAt.ActionName);
            Assert.Equal(created.Id, createdAt.RouteValues?["id"]);
            Assert.Same(created, createdAt.Value);
            ticketService.Verify(s => s.CreateAsync(create, ct), Times.Once);
        }

        [Fact]
        public async Task GetTicketById_TicketExists_ReturnsOkWithTicket()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 42;
            var dto = new TicketReadDto
            {
                Id = id,
                BookingId = 1,
                FlightScheduleId = 10,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com",
                BasePrice = 100,
                Taxes = 20,
                TotalPrice = 120,
                Currency = "USD",
                IsRefundable = false
            };
            ticketService.Setup(s => s.GetByIdAsync(id, ct)).ReturnsAsync(dto);

            // Act
            var actionResult = await controller.GetTicketById(id, ct);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            Assert.Same(dto, ok.Value);
            ticketService.Verify(s => s.GetByIdAsync(id, ct), Times.Once);
        }

        [Fact]
        public async Task GetTicketById_TicketMissing_ReturnsNotFound()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 999;
            ticketService.Setup(s => s.GetByIdAsync(id, ct)).ReturnsAsync((TicketReadDto?)null);

            // Act
            var actionResult = await controller.GetTicketById(id, ct);

            // Assert
            Assert.IsType<NotFoundResult>(actionResult.Result);
            ticketService.Verify(s => s.GetByIdAsync(id, ct), Times.Once);
        }

        [Fact]
        public async Task GetTicketsByFlight_ValidRequest_ReturnsOkWithTickets()
        {
            // Arrange
            var ct = CancellationToken.None;
            var flightId = 10;
            var expected = new List<TicketReadDto>
            {
                new TicketReadDto
                {
                    Id = 1, BookingId = 1, FlightScheduleId = flightId, FareClass = "Y",
                    SeatNumber = "12A", PassengerFullName = "John Doe", PassengerEmail = "john.doe@example.com",
                    BasePrice = 100, Taxes = 20, TotalPrice = 120, Currency = "USD", IsRefundable = true
                }
            };
            ticketService.Setup(s => s.GetTicketsByFlightIdAsync(flightId, ct)).ReturnsAsync(expected);

            // Act
            var actionResult = await controller.GetTicketsByFlight(flightId, ct);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(actionResult.Result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            Assert.Same(expected, ok.Value);
            ticketService.Verify(s => s.GetTicketsByFlightIdAsync(flightId, ct), Times.Once);
        }

        [Fact]
        public async Task DeleteTicket_ValidId_ReturnsNoContent()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 88;
            ticketService.Setup(s => s.DeleteAsync(id, ct)).Returns(Task.CompletedTask);

            // Act
            var result = await controller.DeleteTicket(id, ct);

            // Assert
            Assert.IsType<NoContentResult>(result);
            ticketService.Verify(s => s.DeleteAsync(id, ct), Times.Once);
        }
    }
}