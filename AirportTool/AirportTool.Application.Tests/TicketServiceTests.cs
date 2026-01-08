using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.Contracts.Services;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.Ticket;
using AirportTool.Application.Services;
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
    public class TicketServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IPricingService> pricingService = new();
        private readonly Mock<IMapper> mapper = new();

        private readonly Mock<ITicketRepository> ticketRepository = new();
        private readonly Mock<IFlightScheduleRepository> flightScheduleRepository = new();

        private readonly TicketService ticketService;

        public TicketServiceTests()
        {
            unitOfWork.Setup(u=>u.Tickets).Returns(ticketRepository.Object);
            unitOfWork.Setup(u=>u.FlightSchedules).Returns(flightScheduleRepository.Object);

            ticketService = new TicketService(unitOfWork.Object, pricingService.Object, mapper.Object);
        }

        [Fact]
        public void CreateBookingAsync_QuantityExceedsAircraftCapacity_ThrowsBadRequestException()
        {
            // Arrange
            TicketCreateDto dto = new TicketCreateDto
            {
                BookingId = 1,
                FlightScheduleId = 123,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com"
            };

            flightScheduleRepository.Setup(r => r.GetSeatCapacityAsync(dto.FlightScheduleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);

            ticketRepository.Setup(u=>u.CountByFlightScheduleIdAsync(dto.FlightScheduleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(10);

            // Act & Assert
            var exception = Assert.ThrowsAsync<BadRequestException>(async () =>
                await ticketService.CreateAsync(dto));

            Assert.Equal("No seats available for this flight.", exception.Result.Message);
        }

    }
}
