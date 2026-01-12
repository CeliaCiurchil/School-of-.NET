using AirportTool.Application.ModelDto.Flight;
using FluentValidation;

namespace AirportTool.Application.Validators.Flight
{
    public class FlightUpdateDtoValidator : AbstractValidator<FlightUpdateDto>
    {
        public FlightUpdateDtoValidator()
        {
            Include(new BaseFlightDtoValidator());
        }
    }
}
