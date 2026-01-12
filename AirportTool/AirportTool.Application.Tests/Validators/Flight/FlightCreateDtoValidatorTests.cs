using AirportTool.Application.ModelDto.Flight;
using AirportTool.Application.Validators.Flight;
using FluentValidation.TestHelper;
using Xunit;

namespace AirportTool.Application.Tests.Validators.Flight
{
    public class FlightCreateDtoValidatorTests
    {
        private readonly FlightCreateDtoValidator validator = new();

        private static FlightCreateDto CreateValidDto() => new()
        {
            AirlineIata = "RO",
            FlightNumber = "RO391",
            OriginIata = "OTP",
            DestinationIata = "LHR",
            DefaultAircraftTail = "YR-BGA",
            IsActive = true
        };

        [Fact]
        public void Validate_ValidDto_NoValidationErrors()
        {
            // Arrange
            var validDto = CreateValidDto();

            // Act
            var result = validator.TestValidate(validDto);

            // Assert
            Assert.True(result.IsValid);
            Assert.Empty(result.Errors);
        }

        [Theory]
        [InlineData("R")]
        [InlineData("ro")]
        [InlineData("R1")]
        [InlineData("ROO")]
        public void Validate_AirlineIataInvalid_ReturnsError(string airline)
        {
            // Arrange
            var dto = CreateValidDto();
            dto.AirlineIata = airline;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.AirlineIata));
        }

        [Theory]
        [InlineData("OT")]
        [InlineData("otp")]
        [InlineData("O1P")]
        public void Validate_OriginIataInvalid_ReturnsError(string origin)
        {
            // Arrange
            var dto = CreateValidDto();
            dto.OriginIata = origin;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.OriginIata));
        }

        [Theory]
        [InlineData("lhR")]
        [InlineData("LH")]
        [InlineData("L1R")]
        public void Validate_DestinationIataInvalid_ReturnsError(string destination)
        {
            // Arrange
            var dto = CreateValidDto();
            dto.DestinationIata = destination;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.DestinationIata));
        }

        [Fact]
        public void Validate_OriginMatchesDestination_ReturnsError()
        {
            // Arrange
            var dto = CreateValidDto();
            dto.DestinationIata = dto.OriginIata;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.NotEmpty(result.Errors);
        }

        [Theory]
        [InlineData("391RO")]
        [InlineData("RO1234567")]
        public void Validate_FlightNumberInvalid_ReturnsError(string flightNumber)
        {
            // Arrange
            var dto = CreateValidDto();
            dto.FlightNumber = flightNumber;

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.FlightNumber));
        }

        [Fact]
        public void Validate_DefaultAircraftTailInvalid_ReturnsError()
        {
            // Arrange
            var dto = CreateValidDto();
            dto.DefaultAircraftTail = "YR-ABC#";

            // Act
            var result = validator.TestValidate(dto);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == nameof(dto.DefaultAircraftTail));
        }
    }
}