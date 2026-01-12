using AirportTool.Application.Contracts.Repositories;
using AirportTool.Application.ModelDto.Users;
using HotelListing.API.Models.Users;
using AirportTool.WebApi.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.WebApi.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IAuthManager> auth = new();
        private readonly Mock<ILogger<AccountController>> logger = new();

        private readonly AccountController controller;

        public AccountControllerTests()
        {
            controller = new AccountController(auth.Object, logger.Object);
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsOkWithAuthResponse()
        {
            // Arrange
            var dto = new ApiUserLoginDto { Email = "user@example.com", Password = "secret123" };
            var expected = new AuthResponseDto { UserId = "u1", Token = "jwt-token" };
            auth.Setup(a => a.Login(dto)).ReturnsAsync(expected);

            // Act
            var result = await controller.Login(dto);

            // Assert
            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
            Assert.Same(expected, ok.Value);
            auth.Verify(a => a.Login(dto), Times.Once);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var dto = new ApiUserLoginDto { Email = "wrong@example.com", Password = "wrongpass" };
            auth.Setup(a => a.Login(dto)).ReturnsAsync((AuthResponseDto?)null);

            // Act
            var result = await controller.Login(dto);

            // Assert
            Assert.IsType<UnauthorizedResult>(result);
            auth.Verify(a => a.Login(dto), Times.Once);
        }

        [Fact]
        public async Task Login_Exception_ReturnsProblem500()
        {
            // Arrange
            var dto = new ApiUserLoginDto { Email = "user@example.com", Password = "secret123" };
            auth.Setup(a => a.Login(dto)).ThrowsAsync(new Exception(""));

            // Act
            var result = await controller.Login(dto);

            // Assert
            var problem = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, problem.StatusCode);
            Assert.IsType<ProblemDetails>(problem.Value);
        }

        [Fact]
        public async Task Register_Success_ReturnsOk()
        {
            // Arrange
            var dto = new ApiUserDto { Email = "user@example.com", Password = "secret123", FirstName = "Jane", LastName = "Doe" };
            var response = new RegistrationResponseDto { Succeeded = true, Errors = new List<RegistrationErrorDto>() };
            auth.Setup(a => a.RegisterUser(dto)).ReturnsAsync(response);

            // Act
            var result = await controller.Register(dto);

            // Assert
            Assert.IsType<OkResult>(result);
            auth.Verify(a => a.RegisterUser(dto), Times.Once);
        }

        [Fact]
        public async Task Register_WithErrors_ReturnsBadRequestWithValidationDetails()
        {
            // Arrange
            var dto = new ApiUserDto { Email = "user@example.com", Password = "short", FirstName = "", LastName = "" };
            var response = new RegistrationResponseDto
            {
                Succeeded = false,
                Errors = new List<RegistrationErrorDto>
                {
                    new RegistrationErrorDto { Code = "Password", Description = "Too short" },
                    new RegistrationErrorDto { Code = "FirstName", Description = "Required" }
                }
            };
            auth.Setup(a => a.RegisterUser(dto)).ReturnsAsync(response);

            // Act
            var result = await controller.Register(dto);

            // Assert
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal(StatusCodes.Status400BadRequest, bad.StatusCode);
            var details = Assert.IsType<SerializableError>(bad.Value);
            Assert.True(details.ContainsKey("Password"));
            Assert.True(details.ContainsKey("FirstName"));
            auth.Verify(a => a.RegisterUser(dto), Times.Once);
        }

        [Fact]
        public async Task Register_Exception_ReturnsProblem500()
        {
            // Arrange
            var dto = new ApiUserDto { Email = "user@example.com", Password = "secret123", FirstName = "Jane", LastName = "Doe" };
            auth.Setup(a => a.RegisterUser(dto)).ThrowsAsync(new Exception(""));

            // Act
            var result = await controller.Register(dto);

            // Assert
            var problem = Assert.IsType<ObjectResult>(result);
            Assert.Equal(StatusCodes.Status500InternalServerError, problem.StatusCode);
            Assert.IsType<ProblemDetails>(problem.Value);
        }
    }
}