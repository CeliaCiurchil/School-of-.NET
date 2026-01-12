using AirportTool.Application.Contracts;
using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.Booking;
using AirportTool.Application.Services;
using AirportTool.Domain.Entities;
using AirportTool.Domain.Enums;
using AutoMapper;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.Application.Tests.Services
{
    public class BookingServiceTests
    {
        private readonly Mock<IUnitOfWork> unitOfWork = new();
        private readonly Mock<IMapper> mapper = new();
        private readonly Mock<IBookingRepository> bookingRepository = new();

        private readonly BookingService bookingService;

        public BookingServiceTests()
        {
            unitOfWork.Setup(u => u.Bookings).Returns(bookingRepository.Object);

            bookingService = new BookingService(unitOfWork.Object, mapper.Object);
        }

        [Fact]
        public async Task CancelBookingAsync_BookingDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var confirmationCode = "ABC12345";

            bookingRepository.Setup(r => r.ExistsAsync(confirmationCode, ct))
                .ReturnsAsync(false);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                bookingService.CancelBookingAsync(confirmationCode, ct));

            bookingRepository.Verify(r => r.CancelAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task CancelBookingAsync_BookingExists_CancelsBooking()
        {
            // Arrange
            var ct = CancellationToken.None;
            var confirmationCode = "ABC12345";

            bookingRepository.Setup(r => r.ExistsAsync(confirmationCode, ct))
                .ReturnsAsync(true);

            bookingRepository.Setup(r => r.CancelAsync(confirmationCode, ct))
                .Returns(Task.CompletedTask);

            // Act
            await bookingService.CancelBookingAsync(confirmationCode, ct);

            // Assert
            bookingRepository.Verify(r => r.ExistsAsync(confirmationCode, ct), Times.Once);
            bookingRepository.Verify(r => r.CancelAsync(confirmationCode, ct), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ValidInput_AddsBookingAndReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var dto = new BookingCreateDto { UserId = "user-1" };

            Booking? addedBooking = null;
            bookingRepository
                .Setup(r => r.AddAsync(It.IsAny<Booking>(), ct))
                .Callback<Booking, CancellationToken>((booking, _) => addedBooking = booking)
                .ReturnsAsync((Booking booking, CancellationToken _) =>
                {
                    booking.Id = 42;
                    return booking;
                });

            var expectedDto = new BookingReadDto { Id = 42, UserId = dto.UserId };
            mapper.Setup(m => m.Map<BookingReadDto>(It.IsAny<Booking>()))
                .Returns(expectedDto);

            var before = DateTime.UtcNow;

            // Act
            var result = await bookingService.CreateAsync(dto, ct);

            var after = DateTime.UtcNow;

            // Assert
            Assert.Same(expectedDto, result);
            Assert.NotNull(addedBooking);
            Assert.Equal(dto.UserId, addedBooking!.UserId);
            Assert.Equal((int)BookingStatus.Active, addedBooking.BookingStatusId);
            Assert.Equal(0, addedBooking.Quantity);
            Assert.False(string.IsNullOrWhiteSpace(addedBooking.ConfirmationCode));
            Assert.Equal(8, addedBooking.ConfirmationCode.Length);
            Assert.Equal(addedBooking.ConfirmationCode, addedBooking.ConfirmationCode.ToUpperInvariant());
            Assert.True(addedBooking.CreatedUtc >= before);
            Assert.True(addedBooking.CreatedUtc <= after);

            bookingRepository.Verify(r => r.AddAsync(It.Is<Booking>(b =>
                b.UserId == dto.UserId &&
                b.BookingStatusId == (int)BookingStatus.Active &&
                b.Quantity == 0), ct), Times.Once);
            mapper.Verify(m => m.Map<BookingReadDto>(It.Is<Booking>(b => b.Id == 42)), Times.Once);
        }

        [Fact]
        public async Task GetByConfirmationAsync_BookingExists_ReturnsMappedDto()
        {
            // Arrange
            var ct = CancellationToken.None;
            var confirmationCode = "ABC12345";

            var booking = new Booking { Id = 10, ConfirmationCode = confirmationCode };
            var expectedDto = new BookingReadDto { Id = 10, ConfirmationCode = confirmationCode };

            bookingRepository.Setup(r => r.GetByCodeAsync(confirmationCode, ct))
                .ReturnsAsync(booking);

            mapper.Setup(m => m.Map<BookingReadDto>(booking))
                .Returns(expectedDto);

            // Act
            var result = await bookingService.GetByConfirmationAsync(confirmationCode, ct);

            // Assert
            Assert.Same(expectedDto, result);
            bookingRepository.Verify(r => r.GetByCodeAsync(confirmationCode, ct), Times.Once);
            mapper.Verify(m => m.Map<BookingReadDto>(booking), Times.Once);
        }

        [Fact]
        public async Task GetByConfirmationAsync_BookingDoesNotExist_ThrowsNotFoundException()
        {
            // Arrange
            var ct = CancellationToken.None;
            var confirmationCode = "ABC12345";

            bookingRepository.Setup(r => r.GetByCodeAsync(confirmationCode, ct))
                .ReturnsAsync((Booking?)null);

            // Act & Assert
            await Assert.ThrowsAsync<NotFoundException>(() =>
                bookingService.GetByConfirmationAsync(confirmationCode, ct));

            mapper.Verify(m => m.Map<BookingReadDto>(It.IsAny<Booking>()), Times.Never);
        }
    }
}
