using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.Contracts.Services;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.Ticket;
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
            unitOfWork.Setup(u => u.Tickets).Returns(ticketRepository.Object);
            unitOfWork.Setup(u => u.FlightSchedules).Returns(flightScheduleRepository.Object);

            ticketService = new TicketService(unitOfWork.Object, pricingService.Object, mapper.Object);
        }

        [Fact]
        public async Task CreateAsync_SeatCapacityMissing_ThrowsBadRequestException()
        {
            // Arrange
            var ct = CancellationToken.None;

            TicketCreateDto dto = new TicketCreateDto
            {
                BookingId = 1,
                FlightScheduleId = 123,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com"
            };

            flightScheduleRepository.Setup(r => r.GetSeatCapacityAsync(dto.FlightScheduleId, ct))
                .ReturnsAsync((int?)null);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                ticketService.CreateAsync(dto, ct));

            Assert.Equal("Flight schedule has no aircraft assigned, cannot sell tickets.", exception.Message);

            flightScheduleRepository.Verify(r => r.GetSeatCapacityAsync(dto.FlightScheduleId, ct), Times.Once);
            ticketRepository.Verify(r => r.CountByFlightScheduleIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            pricingService.Verify(p => p.PriceTicketAsync(It.IsAny<PriceTicketRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            ticketRepository.Verify(r => r.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_QuantityExceedsAircraftCapacity_ThrowsBadRequestException()
        {
            // Arrange
            var ct = CancellationToken.None;

            TicketCreateDto dto = new TicketCreateDto
            {
                BookingId = 1,
                FlightScheduleId = 123,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com"
            };

            flightScheduleRepository.Setup(r => r.GetSeatCapacityAsync(dto.FlightScheduleId, ct))
                .ReturnsAsync(10);

            ticketRepository.Setup(u => u.CountByFlightScheduleIdAsync(dto.FlightScheduleId, ct))
                .ReturnsAsync(10);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
                ticketService.CreateAsync(dto, ct));

            Assert.Equal("No seats available for this flight.", exception.Message);

            flightScheduleRepository.Verify(r => r.GetSeatCapacityAsync(dto.FlightScheduleId, ct), Times.Once);
            ticketRepository.Verify(u => u.CountByFlightScheduleIdAsync(dto.FlightScheduleId, ct), Times.Once);
            pricingService.Verify(p => p.PriceTicketAsync(It.IsAny<PriceTicketRequest>(), It.IsAny<CancellationToken>()), Times.Never);
            ticketRepository.Verify(r => r.AddAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ValidInput_ReturnsMappedDtoAndSavesTicket()
        {
            // Arrange
            var ct = CancellationToken.None;

            TicketCreateDto dto = new TicketCreateDto
            {
                BookingId = 1,
                FlightScheduleId = 123,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com"
            };

            var mappedTicket = new Ticket
            {
                BookingId = dto.BookingId,
                FlightScheduleId = dto.FlightScheduleId,
                FareClass = dto.FareClass,
                SeatNumber = dto.SeatNumber,
                PassengerFullName = dto.PassengerFullName,
                PassengerEmail = dto.PassengerEmail
            };

            var prices = new PriceBreakdown(100m, 20m, 120m, "USD", true);

            flightScheduleRepository.Setup(r => r.GetSeatCapacityAsync(dto.FlightScheduleId, ct))
                .ReturnsAsync(150);

            ticketRepository.Setup(r => r.CountByFlightScheduleIdAsync(dto.FlightScheduleId, ct))
                .ReturnsAsync(12);

            pricingService.Setup(p => p.PriceTicketAsync(It.Is<PriceTicketRequest>(r => r.FareClass == 'Y'), ct))
                .ReturnsAsync(prices);

            mapper.Setup(m => m.Map<Ticket>(dto)).Returns(mappedTicket);

            Ticket? addedTicket = null;
            ticketRepository.Setup(r => r.AddAsync(It.IsAny<Ticket>(), ct))
                .ReturnsAsync((Ticket ticket, CancellationToken _) =>
                {
                    addedTicket = ticket;
                    ticket.Id = 42;
                    return ticket;
                });

            unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            var expectedDto = new TicketReadDto { Id = 42 };
            mapper.Setup(m => m.Map<TicketReadDto>(It.IsAny<Ticket>())).Returns(expectedDto);

            // Act
            var result = await ticketService.CreateAsync(dto, ct);

            // Assert
            Assert.Same(expectedDto, result);
            Assert.NotNull(addedTicket);
            Assert.Equal(prices.BasePrice, addedTicket!.BasePrice);
            Assert.Equal(prices.Taxes, addedTicket.Taxes);
            Assert.Equal(prices.TotalPrice, addedTicket.TotalPrice);
            Assert.Equal(prices.Currency, addedTicket.Currency);
            Assert.Equal(prices.IsRefundable, addedTicket.IsRefundable);

            pricingService.Verify(p => p.PriceTicketAsync(It.Is<PriceTicketRequest>(r => r.FareClass == 'Y'), ct), Times.Once);
            ticketRepository.Verify(r => r.AddAsync(It.Is<Ticket>(t =>
                t.BasePrice == prices.BasePrice &&
                t.Taxes == prices.Taxes &&
                t.TotalPrice == prices.TotalPrice &&
                t.Currency == prices.Currency &&
                t.IsRefundable == prices.IsRefundable), ct), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
            mapper.Verify(m => m.Map<TicketReadDto>(It.Is<Ticket>(t => t.Id == 42)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_TicketDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 5;

            ticketRepository.Setup(r => r.ExistsAsync(id, ct))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => ticketService.DeleteAsync(id, ct));

            ticketRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_TicketExists_DeletesAndSaves()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 5;

            ticketRepository.Setup(r => r.ExistsAsync(id, ct))
                .ReturnsAsync(true);

            ticketRepository.Setup(r => r.DeleteAsync(id, ct))
                .Returns(Task.CompletedTask);

            unitOfWork.Setup(u => u.SaveChangesAsync()).ReturnsAsync(1);

            // Act
            await ticketService.DeleteAsync(id, ct);

            // Assert
            ticketRepository.Verify(r => r.DeleteAsync(id, ct), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_TicketExists_ReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 7;

            var ticket = new Ticket { Id = id, FlightScheduleId = 123 };
            var expectedDto = new TicketReadDto { Id = id, FlightScheduleId = 123 };

            ticketRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(ticket);

            mapper.Setup(m => m.Map<TicketReadDto>(ticket))
                .Returns(expectedDto);

            // Act
            var result = await ticketService.GetByIdAsync(id, ct);

            // Assert
            Assert.Equal(id, result.Id);
            Assert.Equal(123, result.FlightScheduleId);
        }

        [Fact]
        public async Task GetByIdAsync_TicketDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 999;

            ticketRepository.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Ticket?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => ticketService.GetByIdAsync(id, ct));

            mapper.Verify(m => m.Map<TicketReadDto>(It.IsAny<Ticket>()), Times.Never);
        }

        [Fact]
        public async Task GetTicketsByFlightIdAsync_TicketsExist_ReturnsMappedDtos()
        {
            // Arrange
            var ct = CancellationToken.None;
            var flightId = 21;

            var tickets = new List<Ticket>
            {
                new Ticket { Id = 1, FlightScheduleId = flightId },
                new Ticket { Id = 2, FlightScheduleId = flightId }
            };

            var expectedDtos = new List<TicketReadDto>
            {
                new TicketReadDto { Id = 1, FlightScheduleId = flightId },
                new TicketReadDto { Id = 2, FlightScheduleId = flightId }
            };

            ticketRepository.Setup(r => r.GetByFlightIdAsync(flightId, ct))
                .ReturnsAsync(tickets);

            mapper.Setup(m => m.Map<IEnumerable<TicketReadDto>>(tickets))
                .Returns(expectedDtos);

            // Act
            var result = await ticketService.GetTicketsByFlightIdAsync(flightId, ct);

            // Assert
            Assert.Same(expectedDtos, result);
            ticketRepository.Verify(r => r.GetByFlightIdAsync(flightId, ct), Times.Once);
            mapper.Verify(m => m.Map<IEnumerable<TicketReadDto>>(tickets), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_TicketDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 8;

            var updateDto = new TicketUpdateDto
            {
                BookingId = 1,
                FlightScheduleId = 123,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com"
            };

            ticketRepository.Setup(r => r.ExistsAsync(id, ct))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => ticketService.UpdateAsync(id, updateDto, ct));

            ticketRepository.Verify(r => r.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()), Times.Never);
            mapper.Verify(m => m.Map<TicketReadDto>(It.IsAny<Ticket>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_TicketExists_UpdatesAndReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 8;

            var updateDto = new TicketUpdateDto
            {
                BookingId = 1,
                FlightScheduleId = 123,
                FareClass = "Y",
                SeatNumber = "12A",
                PassengerFullName = "John Doe",
                PassengerEmail = "john.doe@example.com"
            };

            var mappedTicket = new Ticket
            {
                BookingId = updateDto.BookingId,
                FlightScheduleId = updateDto.FlightScheduleId,
                FareClass = updateDto.FareClass,
                SeatNumber = updateDto.SeatNumber,
                PassengerFullName = updateDto.PassengerFullName,
                PassengerEmail = updateDto.PassengerEmail
            };

            ticketRepository.Setup(r => r.ExistsAsync(id, ct))
                .ReturnsAsync(true);

            mapper.Setup(m => m.Map<Ticket>(updateDto)).Returns(mappedTicket);

            ticketRepository.Setup(r => r.UpdateAsync(It.IsAny<Ticket>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            var expectedDto = new TicketReadDto { Id = id, FlightScheduleId = updateDto.FlightScheduleId };
            mapper.Setup(m => m.Map<TicketReadDto>(It.IsAny<Ticket>())).Returns(expectedDto);

            // Act
            var result = await ticketService.UpdateAsync(id, updateDto, ct);

            // Assert
            Assert.Equal(id, result.Id);
            ticketRepository.Verify(r => r.UpdateAsync(It.Is<Ticket>(t => t.Id == id), It.IsAny<CancellationToken>()), Times.Once);
            mapper.Verify(m => m.Map<TicketReadDto>(It.Is<Ticket>(t => t.Id == id)), Times.Once);
        }
    }
}
