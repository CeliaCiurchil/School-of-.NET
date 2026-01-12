using AirportTool.Application.ModelDto.Flight;
using FluentValidation;

namespace AirportTool.Application.Validators.Flight
{
    public class FlightCreateDtoValidator : AbstractValidator<FlightCreateDto>
    {
        public FlightCreateDtoValidator()
        {
            Include(new BaseFlightDtoValidator());
        }
    }
}
