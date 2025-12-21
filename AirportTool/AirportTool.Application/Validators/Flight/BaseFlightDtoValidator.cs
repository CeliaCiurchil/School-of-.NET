using AirportTool.Application.ModelDto.Flight;
using FluentValidation;

namespace AirportTool.Application.Validators.Flight
{
    public class BaseFlightDtoValidator : AbstractValidator<BaseFlightDto>
    {
        public BaseFlightDtoValidator()
        {
            RuleFor(x => x.AirlineId)
                .GreaterThan(0);

            RuleFor(x => x.FlightNumber)
                .NotEmpty()
                .MaximumLength(8)
                .Matches("^[A-Za-z]+[0-9]+$")
                .WithMessage("FlightNumber must contain letters followed by digits.");

            RuleFor(x => x.OriginAirportId)
                .GreaterThan(0);

            RuleFor(x => x.DestinationAirportId)
                .GreaterThan(0);

            RuleFor(x => x)
                .Must(x => x.OriginAirportId != x.DestinationAirportId)
                .WithMessage("OriginAirportId must be different from DestinationAirportId.");

            RuleFor(x => x.DefaultAircraftId)
                .Must(id => id is null || id > 0)
                .WithMessage("DefaultAircraftId must be greater than 0 when provided.");
        }
    }
}
