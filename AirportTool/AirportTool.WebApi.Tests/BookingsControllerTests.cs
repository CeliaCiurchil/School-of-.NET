using AirportTool.Application.Contracts.Services;
using AirportTool.Application.ModelDto.Booking;
using AirportTool.Application.Services;
using AirportTool.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;
using Xunit;

namespace AirportTool.WebApi.Tests
{
    public partial class BookingsControllerTests
    {
        private readonly Mock<IBookingService> bookingService = new();
        private readonly BookingController bookingController;
        public BookingsControllerTests() 
        {
            bookingController = new BookingController(bookingService.Object);
        }

        [Fact]
        public async Task Create_UserHasUidClaim_ReturnsCreatedAtActionAndCallsServiceOnce()
        {
            // Arrange
            var user = new ClaimsPrincipal(new ClaimsIdentity(
            [
                new Claim("uid", "user-123"),
                new Claim(ClaimTypes.Role, "Client")
            ], authenticationType: "Test"));

            bookingController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext { User = user }
            };

            var expected = new BookingReadDto
            {
                ConfirmationCode = "Q7H2K9",
                BookingStatusId = 1,
                CreatedUtc = DateTime.UtcNow,
                Quantity = 2,
                UserId = "user-123"
            };

            bookingService
                .Setup(s => s.CreateAsync(It.IsAny<BookingCreateDto>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expected);

            // Act
            var result = await bookingController.Create(CancellationToken.None);

            //Assert
            var created = Assert.IsType<CreatedAtActionResult>(result.Result);
            Assert.Equal(201, created.StatusCode);
            Assert.Equal(nameof(BookingController.GetByConfirmationCode), created.ActionName);

            Assert.NotNull(created.RouteValues);
            Assert.Equal("Q7H2K9", created.RouteValues["confirmationCode"]);

            var body = Assert.IsType<BookingReadDto>(created.Value);
            Assert.Equal("Q7H2K9", body.ConfirmationCode);

            bookingService.Verify(s =>
                s.CreateAsync(It.Is<BookingCreateDto>(d => d.UserId == "user-123"),
                              It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }

}
