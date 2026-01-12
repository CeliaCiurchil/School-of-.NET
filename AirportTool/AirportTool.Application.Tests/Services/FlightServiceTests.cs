using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.Flight;
using AirportTool.Application.Services;
using AirportTool.Domain.Entities;
using AutoMapper;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.Application.Tests.Services
{
    public class FlightServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IMapper> mapper = new();

        private readonly Mock<IFlightRepository> flightRepository = new();
        private readonly Mock<IAirlineRepository> airlineRepository = new();
        private readonly Mock<IAirportRepository> airportRepository = new();
        private readonly Mock<IAircraftRepository> aircraftRepository = new();

        private readonly FlightService flightService;

        public FlightServiceTests()
        {
            unitOfWork.Setup(u => u.Flights).Returns(flightRepository.Object);
            unitOfWork.Setup(u => u.Airlines).Returns(airlineRepository.Object);
            unitOfWork.Setup(u => u.Airports).Returns(airportRepository.Object);
            unitOfWork.Setup(u => u.Aircrafts).Returns(aircraftRepository.Object);

            flightService = new FlightService(unitOfWork.Object, mapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_FlightsExist_ReturnsMappedDtos()
        {
            // Arrange
            var ct = CancellationToken.None;
            var flights = new List<Flight>
            {
                new Flight { Id = 1, FlightNumber = "RO101" },
                new Flight { Id = 2, FlightNumber = "RO102" }
            };

            var expectedDtos = new List<FlightReadDto>
            {
                new FlightReadDto { Id = 1, FlightNumber = "RO101" },
                new FlightReadDto { Id = 2, FlightNumber = "RO102" }
            };

            flightRepository.Setup(r => r.GetAllAsync(ct))
                .ReturnsAsync(flights);

            mapper.Setup(m => m.Map<IEnumerable<FlightReadDto>>(flights))
                .Returns(expectedDtos);

            // Act
            var result = await flightService.GetAllAsync(ct);

            // Assert
            Assert.Same(expectedDtos, result);
            flightRepository.Verify(r => r.GetAllAsync(ct), Times.Once);
            mapper.Verify(m => m.Map<IEnumerable<FlightReadDto>>(flights), Times.Once);
        }

        [Fact]
        public async Task GetByIdAsync_FlightExists_ReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 7;

            var flight = new Flight { Id = id, FlightNumber = "RO201" };
            var expectedDto = new FlightReadDto { Id = id, FlightNumber = "RO201" };

            flightRepository.Setup(r => r.GetByIdAsync(id, ct))
                .ReturnsAsync(flight);

            mapper.Setup(m => m.Map<FlightReadDto>(flight))
                .Returns(expectedDto);

            // Act
            var result = await flightService.GetByIdAsync(id, ct);

            // Assert
            Assert.Same(expectedDto, result);
        }

        [Fact]
        public async Task GetByIdAsync_FlightDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 999;

            flightRepository.Setup(r => r.GetByIdAsync(id, ct))
                .ReturnsAsync((Flight?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => flightService.GetByIdAsync(id, ct));

            mapper.Verify(m => m.Map<FlightReadDto>(It.IsAny<Flight>()), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_AirlineNotFound_ThrowsBadRequestException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var dto = new FlightCreateDto
            {
                AirlineIata = "XX",
                FlightNumber = "XX100",
                OriginIata = "OTP",
                DestinationIata = "LHR",
                DefaultAircraftTail = "YR-ABC",
                IsActive = true
            };

            airlineRepository.Setup(r => r.GetByIataCodeAsync(dto.AirlineIata, ct))
                .ReturnsAsync((Airline?)null);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                flightService.CreateAsync(dto, ct));

            Assert.Contains("Airline", ex.Message);
            Assert.Contains(dto.AirlineIata, ex.Message);

            airportRepository.Verify(r => r.GetByIataCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            aircraftRepository.Verify(r => r.GetByTailNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            flightRepository.Verify(r => r.AddAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_DuplicateFlightNumber_ThrowsConflictException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var dto = new FlightCreateDto
            {
                AirlineIata = "RO",
                FlightNumber = "RO100",
                OriginIata = "OTP",
                DestinationIata = "LHR",
                DefaultAircraftTail = "YR-ABC",
                IsActive = true
            };

            airlineRepository.Setup(r => r.GetByIataCodeAsync(dto.AirlineIata, ct))
                .ReturnsAsync(new Airline { Id = 1 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.OriginIata, ct))
                .ReturnsAsync(new Airport { Id = 10 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.DestinationIata, ct))
                .ReturnsAsync(new Airport { Id = 20 });

            aircraftRepository.Setup(r => r.GetByTailNumberAsync(dto.DefaultAircraftTail!, ct))
                .ReturnsAsync(new Aircraft { Id = 5 });

            flightRepository.Setup(r => r.ExistsByAirlineAndNumberAsync(1, dto.FlightNumber, ct))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ConflictException>(() =>
                flightService.CreateAsync(dto, ct));

            Assert.Contains(dto.FlightNumber, ex.Message);
            Assert.Contains("airline ID 1", ex.Message);

            flightRepository.Verify(r => r.AddAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()), Times.Never);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Never);
        }

        [Fact]
        public async Task CreateAsync_ValidInput_AddsFlightAndReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var dto = new FlightCreateDto
            {
                AirlineIata = "RO",
                FlightNumber = "RO100",
                OriginIata = "OTP",
                DestinationIata = "LHR",
                DefaultAircraftTail = "YR-ABC",
                IsActive = true
            };

            airlineRepository.Setup(r => r.GetByIataCodeAsync(dto.AirlineIata, ct))
                .ReturnsAsync(new Airline { Id = 1 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.OriginIata, ct))
                .ReturnsAsync(new Airport { Id = 10 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.DestinationIata, ct))
                .ReturnsAsync(new Airport { Id = 20 });

            aircraftRepository.Setup(r => r.GetByTailNumberAsync(dto.DefaultAircraftTail!, ct))
                .ReturnsAsync(new Aircraft { Id = 5 });

            flightRepository.Setup(r => r.ExistsByAirlineAndNumberAsync(1, dto.FlightNumber, ct))
                .ReturnsAsync(false);

            var created = new Flight
            {
                Id = 42,
                AirlineId = 1,
                FlightNumber = dto.FlightNumber,
                OriginAirportId = 10,
                DestinationAirportId = 20,
                DefaultAircraftId = 5,
                IsActive = dto.IsActive
            };

            flightRepository.Setup(r => r.AddAsync(It.IsAny<Flight>(), ct))
                .ReturnsAsync(created);

            unitOfWork.Setup(u => u.SaveChangesAsync())
                .ReturnsAsync(1);

            var expectedDto = new FlightReadDto { Id = 42 };
            mapper.Setup(m => m.Map<FlightReadDto>(created))
                .Returns(expectedDto);

            // Act
            var result = await flightService.CreateAsync(dto, ct);

            // Assert
            Assert.Same(expectedDto, result);
            flightRepository.Verify(r => r.AddAsync(It.Is<Flight>(f =>
                f.AirlineId == 1 &&
                f.OriginAirportId == 10 &&
                f.DestinationAirportId == 20 &&
                f.DefaultAircraftId == 5), ct), Times.Once);
            unitOfWork.Verify(u => u.SaveChangesAsync(), Times.Once);
            mapper.Verify(m => m.Map<FlightReadDto>(created), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_FlightDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 123;
            var dto = new FlightUpdateDto
            {
                AirlineIata = "RO",
                FlightNumber = "RO200",
                OriginIata = "OTP",
                DestinationIata = "LHR",
                DefaultAircraftTail = "YR-ABC",
                IsActive = true
            };

            flightRepository.Setup(r => r.GetByIdAsync(id, ct))
                .ReturnsAsync((Flight?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                flightService.UpdateAsync(id, dto, ct));

            flightRepository.Verify(r => r.UpdateAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_DuplicateFlightNumber_ThrowsConflictException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 123;
            var dto = new FlightUpdateDto
            {
                AirlineIata = "RO",
                FlightNumber = "RO200",
                OriginIata = "OTP",
                DestinationIata = "LHR",
                DefaultAircraftTail = "YR-ABC",
                IsActive = true
            };

            flightRepository.Setup(r => r.GetByIdAsync(id, ct))
                .ReturnsAsync(new Flight { Id = id, AirlineId = 99, FlightNumber = "OLD" });

            airlineRepository.Setup(r => r.GetByIataCodeAsync(dto.AirlineIata, ct))
                .ReturnsAsync(new Airline { Id = 1 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.OriginIata, ct))
                .ReturnsAsync(new Airport { Id = 10 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.DestinationIata, ct))
                .ReturnsAsync(new Airport { Id = 20 });

            aircraftRepository.Setup(r => r.GetByTailNumberAsync(dto.DefaultAircraftTail!, ct))
                .ReturnsAsync(new Aircraft { Id = 5 });

            flightRepository.Setup(r => r.ExistsByAirlineAndNumberAsync(1, dto.FlightNumber, ct))
                .ReturnsAsync(true);

            // Act & Assert
            var ex = await Assert.ThrowsAsync<ConflictException>(() =>
                flightService.UpdateAsync(id, dto, ct));

            Assert.Contains(dto.FlightNumber, ex.Message);
            Assert.Contains("airline ID 1", ex.Message);

            flightRepository.Verify(r => r.UpdateAsync(It.IsAny<Flight>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task UpdateAsync_ValidInput_UpdatesAndReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 123;
            var dto = new FlightUpdateDto
            {
                AirlineIata = "RO",
                FlightNumber = "RO200",
                OriginIata = "OTP",
                DestinationIata = "LHR",
                DefaultAircraftTail = null,
                IsActive = false
            };

            flightRepository.Setup(r => r.GetByIdAsync(id, ct))
                .ReturnsAsync(new Flight { Id = id, AirlineId = 1, FlightNumber = "RO200" });

            airlineRepository.Setup(r => r.GetByIataCodeAsync(dto.AirlineIata, ct))
                .ReturnsAsync(new Airline { Id = 1 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.OriginIata, ct))
                .ReturnsAsync(new Airport { Id = 10 });

            airportRepository.Setup(r => r.GetByIataCodeAsync(dto.DestinationIata, ct))
                .ReturnsAsync(new Airport { Id = 20 });

            flightRepository.Setup(r => r.UpdateAsync(It.IsAny<Flight>(), ct))
                .Returns(Task.CompletedTask);

            var expectedDto = new FlightReadDto { Id = id, IsActive = false };
            mapper.Setup(m => m.Map<FlightReadDto>(It.IsAny<Flight>()))
                .Returns(expectedDto);

            // Act
            var result = await flightService.UpdateAsync(id, dto, ct);

            // Assert
            Assert.Same(expectedDto, result);
            flightRepository.Verify(r => r.ExistsByAirlineAndNumberAsync(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            aircraftRepository.Verify(r => r.GetByTailNumberAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            flightRepository.Verify(r => r.UpdateAsync(It.Is<Flight>(f =>
                f.Id == id &&
                f.AirlineId == 1 &&
                f.OriginAirportId == 10 &&
                f.DestinationAirportId == 20 &&
                f.DefaultAircraftId == null &&
                f.IsActive == false), ct), Times.Once);
            mapper.Verify(m => m.Map<FlightReadDto>(It.Is<Flight>(f => f.Id == id)), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_FlightDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 55;

            flightRepository.Setup(r => r.ExistsAsync(id, ct))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() => flightService.DeleteAsync(id, ct));

            flightRepository.Verify(r => r.DeleteAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task DeleteAsync_FlightExists_DeletesAndReturnsTrue()
        {
            // Arrange
            var ct = CancellationToken.None;
            var id = 55;

            flightRepository.Setup(r => r.ExistsAsync(id, ct))
                .ReturnsAsync(true);

            flightRepository.Setup(r => r.DeleteAsync(id, ct))
                .Returns(Task.CompletedTask);

            // Act
            var result = await flightService.DeleteAsync(id, ct);

            // Assert
            Assert.True(result);
            flightRepository.Verify(r => r.DeleteAsync(id, ct), Times.Once);
        }
    }
}
