using AirportTool.Application.ModelDto.Flight;
using FluentValidation;

namespace AirportTool.Application.Validators.Flight
{
    public class BaseFlightDtoValidator : AbstractValidator<BaseFlightDto>
    {
        public BaseFlightDtoValidator()
        {
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

            RuleFor(x => x.FlightNumber)
                .NotEmpty()
                .MaximumLength(8)
                .Matches("^[A-Za-z]+[0-9]+$")
                .WithMessage("FlightNumber must be letters+digits (e.g., RO391) and max 8 chars.");

            RuleFor(x => x.DefaultAircraftTail)
                .MaximumLength(10)
                .Matches("^[A-Z0-9-]+$")
                .When(x => !string.IsNullOrWhiteSpace(x.DefaultAircraftTail))
                .WithMessage("DefaultAircraftTail contains invalid characters.");
        }
    }
}
