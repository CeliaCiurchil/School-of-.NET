using AirportTool.Application.Contracts.Services;
using AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport;
using AirportTool.Application.Options;
using AirportTool.WebApi.Controllers;
using AirportTool.WebApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace AirportTool.WebApi.Tests
{
    public class SchedulesControllerTests
    {
        private readonly Mock<IFlightScheduleService> flightScheduleService = new();
        private readonly Mock<IScheduleImportService> scheduleImportService = new();

        private static IOptions<ScheduleImportOptions> CreateImportOptions()
        {
            return Microsoft.Extensions.Options.Options.Create(new ScheduleImportOptions
            {
                MaxFileSizeBytes = 5_000_000,
                MaxRows = 1000
            });
        }

        private static IFormFile CreateJsonFormFile(string json, string fileName = "schedules.json")
        {
            var bytes = Encoding.UTF8.GetBytes(json);
            var stream = new MemoryStream(bytes);

            // FormFile needs a stream + length + name + fileName
            return new FormFile(stream, 0, bytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/json"
            };
        }

        [Fact]
        public async Task ImportFlightSchedules_SomeRowsFail_Returns207MultiStatus()
        {
            // Arrange
            var controller = new SchedulesController(
                flightScheduleService.Object,
                scheduleImportService.Object,
                CreateImportOptions());

            var file = CreateJsonFormFile(@"[{""flightNumber"":""RO391""}]");
            var request = new FileImportRequest { File = file };

            var summary = new ScheduleImportSummaryDto
            {
                Total = 2,
                Created = 1,
                Updated = 0,
                Errors = { new ScheduleImportErrorDto { Row = 2, Message = "Gate overlap" } }
            };

            scheduleImportService
                .Setup(s => s.ImportAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(summary);

            // Act
            var result = await controller.ImportFlightSchedules(request, CancellationToken.None);

            // Assert: StatusCode(207, summary)
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status207MultiStatus, objectResult.StatusCode);

            var body = Assert.IsType<ScheduleImportSummaryDto>(objectResult.Value);
            Assert.Equal(2, body.Total);
            Assert.Single(body.Errors);

            scheduleImportService.Verify(
                s => s.ImportAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }

        [Fact]
        public async Task ImportFlightSchedules_AllRowsCreated_Returns201Created()
        {
            // Arrange
            var controller = new SchedulesController(
                flightScheduleService.Object,
                scheduleImportService.Object,
                CreateImportOptions());

            var file = CreateJsonFormFile(@"[
              {""flightNumber"":""RO391"",""airlineIata"":""RO"",""originIata"":""OTP"",""destinationIata"":""LHR"",
               ""scheduledDepartureUtc"":""2025-12-01T06:30:00Z"",""scheduledArrivalUtc"":""2025-12-01T08:25:00Z""}
            ]");

            var request = new FileImportRequest { File = file };

            var summary = new ScheduleImportSummaryDto
            {
                Total = 1,
                Created = 1,
                Updated = 0
            };

            scheduleImportService
                .Setup(s => s.ImportAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(summary);

            // Act
            var result = await controller.ImportFlightSchedules(request, CancellationToken.None);

            // Assert
            var objectResult = Assert.IsType<ObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status201Created, objectResult.StatusCode);

            var body = Assert.IsType<ScheduleImportSummaryDto>(objectResult.Value);
            Assert.Equal(1, body.Total);
            Assert.Equal(1, body.Created);
            Assert.Empty(body.Errors);

            scheduleImportService.Verify(
                s => s.ImportAsync(It.IsAny<Stream>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}