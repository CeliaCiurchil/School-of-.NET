using AirportTool.Application.ModelDto.Ticket;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Validators.Ticket
{
    public class TicketCreateDtoValidator : AbstractValidator<TicketCreateDto>
    {
        public TicketCreateDtoValidator()
        {
            RuleFor(x => x.BookingId)
                .GreaterThan(0)
                .WithMessage("BookingId must be a positive number.");

            RuleFor(x => x.FlightScheduleId)
                .GreaterThan(0)
                .WithMessage("FlightScheduleId must be a positive number.");

            RuleFor(x => x.FareClass)
                .NotEmpty()
                .MaximumLength(2) 
                .Must(fc =>
                {
                    var normalized = fc.Trim().ToUpperInvariant();
                    return normalized is "Y" or "M" or "J" or "F";
                })
                .WithMessage("FareClass must be one of: Y, M, J, F.");

            RuleFor(x => x.SeatNumber)
                .NotEmpty()
                .MaximumLength(4)
                .Matches(@"^[0-9]{1,2}[A-Z]?$")
                .WithMessage("SeatNumber must look like 12A, 1B, 10C (max 4 chars).");

            RuleFor(x => x.PassengerFullName)
                .NotEmpty()
                .MaximumLength(120)
                .WithMessage("PassengerFullName is required and max 120 characters.");

            RuleFor(x => x.PassengerEmail)
                .NotEmpty()
                .MaximumLength(120)
                .EmailAddress()
                .WithMessage("PassengerEmail must be a valid email address and max 120 characters.");
        }
    }
}
