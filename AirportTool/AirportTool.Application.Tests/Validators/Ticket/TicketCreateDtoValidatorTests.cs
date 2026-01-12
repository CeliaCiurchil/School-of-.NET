using AirportTool.Application.ModelDto.Ticket;
using AirportTool.Application.Validators.Ticket;
using FluentValidation.TestHelper;
using Xunit;

namespace AirportTool.Application.Tests.Validators.Ticket
{
    public class TicketCreateDtoValidatorTests
    {
        private readonly TicketCreateDtoValidator validator = new();

        private static TicketCreateDto ValidDto() => new()
        {
            BookingId = 1,
            FlightScheduleId = 123,
            FareClass = "Y",
            SeatNumber = "12A",
            PassengerFullName = "John Doe",
            PassengerEmail = "john.doe@example.com"
        };

        [Fact]
        public void Validate_ValidDto_IsValid()
        {
            // Arrange
            var dto = ValidDto();

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_BookingIdNotPositive_ReturnsError(int bookingId)
        {
            // Arrange
            var dto = ValidDto();
            dto.BookingId = bookingId;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.BookingId));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Validate_FlightScheduleIdNotPositive_ReturnsError(int flightScheduleId)
        {
            // Arrange
            var dto = ValidDto();
            dto.FlightScheduleId = flightScheduleId;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.FlightScheduleId));
        }

        [Theory]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("X")]
        [InlineData("Q")]
        [InlineData("YY")]
        [InlineData("ABC")]    
        [InlineData(" y ")]   
        public void Validate_FareClassInvalid_ReturnsError(string? fareClass)
        {
            // Arrange
            var dto = ValidDto();
            dto.FareClass = fareClass;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.FareClass));
        }

        [Theory]
        [InlineData("y")]
        [InlineData("m")]
        [InlineData("J")]
        [InlineData("F")]
        [InlineData("Y ")] 
        public void Validate_FareClassValid_AcceptsNormalization(string fareClass)
        {
            // Arrange
            var dto = ValidDto();
            dto.FareClass = fareClass;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(dto.FareClass));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_SeatNumberEmpty_ReturnsError(string? seat)
        {
            // Arrange
            var dto = ValidDto();
            dto.SeatNumber = seat;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.SeatNumber));
        }

        [Theory]
        [InlineData("12345")] 
        [InlineData("AAA")]
        [InlineData("A12")]
        [InlineData("12AA")]
        [InlineData("12a")]   
        [InlineData("12-")]   
        public void Validate_SeatNumberInvalidFormat_ReturnsError(string seat)
        {
            // Arrange
            var dto = ValidDto();
            dto.SeatNumber = seat;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.SeatNumber));
        }

        [Theory]
        [InlineData("1")]
        [InlineData("12")]
        [InlineData("1A")]
        [InlineData("12A")]
        public void Validate_SeatNumberValidFormats_AreAccepted(string seat)
        {
            // Arrange
            var dto = ValidDto();
            dto.SeatNumber = seat;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.DoesNotContain(result.Errors, e => e.PropertyName == nameof(dto.SeatNumber));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void Validate_PassengerFullNameEmpty_ReturnsError(string? name)
        {
            // Arrange
            var dto = ValidDto();
            dto.PassengerFullName = name;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.PassengerFullName));
        }

        [Fact]
        public void Validate_PassengerFullNameTooLong_ReturnsError()
        {
            // Arrange
            var dto = ValidDto();
            dto.PassengerFullName = new string('A', 121);

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.PassengerFullName));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("not-an-email")]
        [InlineData("a@")]
        [InlineData("@b.com")]
        public void Validate_PassengerEmailInvalid_ReturnsError(string? email)
        {
            // Arrange
            var dto = ValidDto();
            dto.PassengerEmail = email;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.PassengerEmail));
        }

        [Fact]
        public void Validate_PassengerEmailTooLong_ReturnsError()
        {
            // Arrange
            var dto = ValidDto();
            dto.PassengerEmail = new string('a', 121) + "@example.com";

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.PassengerEmail));
        }
    }
}