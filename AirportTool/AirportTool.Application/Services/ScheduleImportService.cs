using AirportTool.Application.Contracts;
using AirportTool.Application.Exceptions;
using AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport;
using AirportTool.Application.Options;
using AirportTool.Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Globalization;
using System.Text.Json;

namespace AirportTool.Application.Services
{
    public class ScheduleImportService : IScheduleImportService
    {
        private const string StatusCreated = "Created";
        private const string StatusUpdated = "Updated";
        private const string StatusError = "Error";
        private const string PlannedStatus = "Planned";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        private readonly IUnitOfWork _unitOfWork;
        private readonly ScheduleImportOptions _options;
        private readonly ILogger<ScheduleImportService> _logger;

        private readonly IValidator<ScheduleImportRowDto> _validator;
        private int? _plannedStatusId;
        private readonly int bufferMinutes;

        public ScheduleImportService(
            IUnitOfWork unitOfWork,
            IOptions<ScheduleImportOptions> options,
            ILogger<ScheduleImportService> logger,
            IValidator<ScheduleImportRowDto> validator,
            int bufferMinutes=30)
        {
            _unitOfWork = unitOfWork;
            _options = options.Value;
            _logger = logger;
            _validator = validator;
            this.bufferMinutes = bufferMinutes;
        }

        public async Task<ScheduleImportSummaryDto> ImportAsync(Stream jsonStream, CancellationToken ct)
        {
            if (jsonStream is null)
            {
                throw new BadRequestException("File stream is missing.");
            }

            JsonDocument document;

            try
            {
                document = await JsonDocument.ParseAsync(jsonStream, cancellationToken: ct);
            }
            catch (JsonException)
            {
                throw new BadRequestException("Invalid JSON file. Please upload a valid JSON array.");
            }

            using (document)
            {
                if (document.RootElement.ValueKind != JsonValueKind.Array)
                {
                    throw new BadRequestException("JSON must be an array of schedules.");
                }

                var total = document.RootElement.GetArrayLength();
                if (total > _options.MaxRows)
                {
                    throw new BadRequestException($"JSON array exceeds maximum allowed rows of {_options.MaxRows}.");
                }

                var summary = new ScheduleImportSummaryDto
                {
                    Total = total
                };

                var rowIndex = 0;
                foreach (var element in document.RootElement.EnumerateArray())
                {
                    ct.ThrowIfCancellationRequested();
                    rowIndex++;

                    var rowResult = new ScheduleImportRowResultDto
                    {
                        Row = rowIndex
                    };

                    try
                    {
                        var rowDto = DeserializeRow(element, rowResult, summary);
                        if (rowDto is null)
                        {
                            continue;
                        }

                        if (!TryNormalizeRow(rowDto, out var normalizedRow, out var validationError))
                        {
                            AddRowError(summary, rowResult, rowIndex, validationError ?? "Row validation failed.");
                            continue;
                        }

                        RowOutcome? outcome = null;
                        outcome = await ProcessRowAsync(normalizedRow, ct);

                        if (outcome is null)
                        {
                            AddRowError(summary, rowResult, rowIndex, "Row processing failed.");
                            continue;
                        }

                        rowResult.Status = outcome.Status;
                        rowResult.FlightId = outcome.FlightId;
                        rowResult.FlightScheduleId = outcome.FlightScheduleId;
                        summary.Results.Add(rowResult);

                        if (outcome.Status == StatusCreated)
                        {
                            summary.Created++;
                        }
                        else if (outcome.Status == StatusUpdated)
                        {
                            summary.Updated++;
                        }
                    }
                    catch (RowImportException ex)
                    {
                        AddRowError(summary, rowResult, rowIndex, ex.Message);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Schedule import row {Row} failed.", rowIndex);
                        AddRowError(summary, rowResult, rowIndex, "Unexpected error processing row.");
                    }
                }

                _logger.LogInformation(
                    "Schedule import completed. Total: {Total}, Created: {Created}, Updated: {Updated}, Errors: {Errors}",
                    summary.Total,
                    summary.Created,
                    summary.Updated,
                    summary.Errors.Count);

                return summary;
            }
        }

        private bool TryNormalizeRow(
            ScheduleImportRowDto rowDto,
            out NormalizedRow? normalizedRow,
            out string? validationError)
        {
            normalizedRow = null;
            validationError = null;

            var validationResult = _validator.Validate(rowDto);
            if (!validationResult.IsValid)
            {
                validationError = string.Join("; ", validationResult.Errors.Select(e => e.ErrorMessage));
                return false;
            }

            normalizedRow = new NormalizedRow(
                FlightNumber: rowDto.FlightNumber!.Trim().ToUpperInvariant(),
                AirlineIata: rowDto.AirlineIata!.Trim().ToUpperInvariant(),
                OriginIata: rowDto.OriginIata!.Trim().ToUpperInvariant(),
                DestinationIata: rowDto.DestinationIata!.Trim().ToUpperInvariant(),
                ScheduledDepartureUtc: rowDto.ScheduledDepartureUtc!.Value,
                ScheduledArrivalUtc: rowDto.ScheduledArrivalUtc!.Value,
                GateCode: NormalizeOptional(rowDto.GateCode),
                AssignedAircraftTail: NormalizeOptional(rowDto.AssignedAircraftTail)
            );

            return true;
        }


        private ScheduleImportRowDto? DeserializeRow(
            JsonElement element,
            ScheduleImportRowResultDto rowResult,
            ScheduleImportSummaryDto summary)
        {
            try
            {
                var rowDto = element.Deserialize<ScheduleImportRowDto>(JsonOptions);
                if (rowDto is null)
                {
                    AddRowError(summary, rowResult, rowResult.Row, "Row is empty.");
                    return null;
                }

                return rowDto;
            }
            catch (JsonException)
            {
                AddRowError(summary, rowResult, rowResult.Row, "Invalid row JSON.");
                return null;
            }
        }

        private async Task<RowOutcome> ProcessRowAsync(NormalizedRow row, CancellationToken ct)
        {
            var airline = await _unitOfWork.Airlines.GetByIataCodeAsync(row.AirlineIata, ct);
            if (airline is null)
            {
                throw new RowImportException($"Airline not found: {row.AirlineIata}.");
            }

            var origin = await _unitOfWork.Airports.GetByIataCodeAsync(row.OriginIata, ct);
            if (origin is null)
            {
                throw new RowImportException($"Origin airport not found: {row.OriginIata}.");
            }

            var destination = await _unitOfWork.Airports.GetByIataCodeAsync(row.DestinationIata, ct);
            if (destination is null)
            {
                throw new RowImportException($"Destination airport not found: {row.DestinationIata}.");
            }

            var flight = await _unitOfWork.Flights.GetByRouteAsync(
                airline.Id,
                row.FlightNumber,
                origin.Id,
                destination.Id,
                ct);

            if (flight is null)
            {
                flight = await _unitOfWork.Flights.AddAsync(new Flight
                {
                    AirlineId = airline.Id,
                    FlightNumber = row.FlightNumber,
                    OriginAirportId = origin.Id,
                    DestinationAirportId = destination.Id,
                    IsActive = true
                }, ct);
            }

            int? gateId = null;
            if (!string.IsNullOrWhiteSpace(row.GateCode))
            {
                gateId = await _unitOfWork.Gates.GetGateIdByCodeAndAirportAsync(origin.Id, row.GateCode, ct);
                if (!gateId.HasValue)
                {
                    throw new RowImportException($"Gate not found: {row.GateCode}.");
                }
            }

            int? aircraftId = null;
            if (!string.IsNullOrWhiteSpace(row.AssignedAircraftTail))
            {
                var aircraft = await _unitOfWork.Aircrafts.GetByTailNumberAsync(row.AssignedAircraftTail, ct);
                if (aircraft is null)
                {
                    throw new RowImportException($"Aircraft not found: {row.AssignedAircraftTail}.");
                }

                aircraftId = aircraft.Id;
            }

            var departureUtc = row.ScheduledDepartureUtc.UtcDateTime;
            var arrivalUtc = row.ScheduledArrivalUtc.UtcDateTime;

            var existingSchedule = await _unitOfWork.FlightSchedules
                .GetByFlightAndDepartureAsync(flight.Id, departureUtc, ct);

            if (gateId.HasValue)
            {
                var hasOverlap = await _unitOfWork.FlightSchedules
                    .HasGateOverlapAsync(gateId.Value, departureUtc,bufferMinutes, existingSchedule?.Id, ct);

                if (hasOverlap)
                {
                    var message = $"Gate overlap at {row.OriginIata}:{row.GateCode} {FormatUtc(departureUtc)}{FormatUtc(arrivalUtc)}";
                    throw new RowImportException(message);
                }
            }

            if (existingSchedule is null)
            {
                var plannedStatusId = await GetPlannedStatusIdAsync(ct);
                var createdSchedule = await _unitOfWork.FlightSchedules.AddAsync(new FlightSchedule
                {
                    FlightId = flight.Id,
                    ScheduledDepartureUtc = departureUtc,
                    ScheduledArrivalUtc = arrivalUtc,
                    GateId = gateId,
                    AssignedAircraftId = aircraftId,
                    StatusId = plannedStatusId
                }, ct);

                return new RowOutcome(StatusCreated, flight.Id, createdSchedule.Id);
            }

            await _unitOfWork.FlightSchedules.UpdateAsync(new FlightSchedule
            {
                Id = existingSchedule.Id,
                FlightId = existingSchedule.FlightId,
                ScheduledDepartureUtc = existingSchedule.ScheduledDepartureUtc,
                ScheduledArrivalUtc = arrivalUtc,
                GateId = gateId,
                AssignedAircraftId = aircraftId,
                StatusId = existingSchedule.StatusId
            }, ct);

            return new RowOutcome(StatusUpdated, flight.Id, existingSchedule.Id);
        }

        private async Task<int> GetPlannedStatusIdAsync(CancellationToken ct)
        {
            if (_plannedStatusId.HasValue)
            {
                return _plannedStatusId.Value;
            }

            var statusId = await _unitOfWork.FlightStatuses.GetIdByStatusAsync(PlannedStatus, ct);
            if (!statusId.HasValue)
            {
                throw new RowImportException("Flight status \"Planned\" not found.");
            }

            _plannedStatusId = statusId.Value;
            return statusId.Value;
        }

        private static string FormatUtc(DateTime value)
        {
            return value.ToString("yyyy-MM-ddTHH:mm:ss'Z'", CultureInfo.InvariantCulture);
        }

        private void AddRowError(
            ScheduleImportSummaryDto summary,
            ScheduleImportRowResultDto rowResult,
            int rowIndex,
            string message)
        {
            _logger.LogWarning("Schedule import row {Row} failed: {Message}", rowIndex, message);
            rowResult.Status = StatusError;
            rowResult.Message = message;
            summary.Errors.Add(new ScheduleImportErrorDto { Row = rowIndex, Message = message });
            summary.Results.Add(rowResult);
        }

        private static string? NormalizeOptional(string? value)
        {
            return string.IsNullOrWhiteSpace(value) ? null : value.Trim().ToUpperInvariant();
        }

        private sealed class RowImportException : Exception
        {
            public RowImportException(string message) : base(message)
            {
            }
        }

        private sealed record NormalizedRow(
            string FlightNumber,
            string AirlineIata,
            string OriginIata,
            string DestinationIata,
            DateTimeOffset ScheduledDepartureUtc,
            DateTimeOffset ScheduledArrivalUtc,
            string? GateCode,
            string? AssignedAircraftTail);

        private sealed record RowOutcome(string Status, int FlightId, int FlightScheduleId);
    }
}
