using AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport;
using FluentValidation;

namespace AirportTool.Application.Validators.FlightSchedule
{
    public class ScheduleImportRowDtoValidator : AbstractValidator<ScheduleImportRowDto>
    {
        public ScheduleImportRowDtoValidator()
        {
            RuleFor(x => x.FlightNumber)
                .NotEmpty()
                .MaximumLength(8)
                .Matches("^[A-Za-z]+[0-9]+$")
                .WithMessage("FlightNumber must be letters+digits (e.g., RO391) and max 8 chars.");

            RuleFor(x => x.AirlineIata)
                .NotEmpty()
                .Length(2)
                .Matches("^[A-Z]{2}$")
                .WithMessage("AirlineIata must be exactly 2 uppercase letters (e.g., RO).");

            RuleFor(x => x.OriginIata)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("OriginIata must be exactly 3 uppercase letters (e.g., OTP).");

            RuleFor(x => x.DestinationIata)
                .NotEmpty()
                .Length(3)
                .Matches("^[A-Z]{3}$")
                .WithMessage("DestinationIata must be exactly 3 uppercase letters (e.g., LHR).");

            RuleFor(x => x)
                .Must(x => x.OriginIata != x.DestinationIata)
                .WithMessage("OriginIata and DestinationIata must be different.");

            RuleFor(x => x.ScheduledDepartureUtc)
                .NotEmpty()
                .WithMessage("ScheduledDepartureUtc is required.");

            RuleFor(x => x.ScheduledArrivalUtc)
                .NotEmpty()
                .WithMessage("ScheduledArrivalUtc is required.")
                .GreaterThan(x => x.ScheduledDepartureUtc)
                .WithMessage("ScheduledArrivalUtc must be after ScheduledDepartureUtc.");

            RuleFor(x => x.GateCode)
                .NotEmpty()
                .Matches("^[A-Z]{1,2}[0-9]{1,3}$")
                .WithMessage("GateCode must be 1-2 letters followed by 1-3 digits (e.g., A12).");

            RuleFor(x => x.AssignedAircraftTail)
                .MaximumLength(10)
                .Matches("^[A-Z0-9-]+$")
                .When(x => !string.IsNullOrWhiteSpace(x.AssignedAircraftTail))
                .WithMessage("AssignedAircraftTail contains invalid characters.");
        }
    }
}
