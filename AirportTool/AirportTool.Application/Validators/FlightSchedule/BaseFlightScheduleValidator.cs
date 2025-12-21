using AirportTool.Application.ModelDto.FlightSchedule;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Validators.FlightSchedule
{
    public class BaseFlightScheduleValidator : AbstractValidator<BaseFlightScheduleDto>
    {
        public BaseFlightScheduleValidator()
        {
            RuleFor(x => x.FlightId)
                .GreaterThan(0)
                .WithMessage("FlightId must be a positive integer.");

            RuleFor(x => x.ScheduledDepartureUtc)
                .NotEmpty()
                .WithMessage("ScheduledDepartureUtc is required.");

            RuleFor(x => x.ScheduledArrivalUtc)
                .NotEmpty()
                .WithMessage("ScheduledArrivalUtc is required.")
                .GreaterThan(x => x.ScheduledDepartureUtc)
                .WithMessage("ScheduledArrivalUtc must be after ScheduledDepartureUtc.");

            RuleFor(x => x.GateId)
                .Must(id => id is null || id > 0)
                .WithMessage("GateId must be null or a positive integer.");

            RuleFor(x => x.AssignedAircraftId)
                .Must(id => id is null || id > 0)
                .WithMessage("AssignedAircraftId must be null or a positive integer.");

            // StatusId is a tinyint enum in the brief (0..4). Your model uses int, so validate range.
            RuleFor(x => x.StatusId)
                .InclusiveBetween(0, 4)
                .WithMessage("StatusId must be between 0 and 4 (Planned=0, Boarding=1, Departed=2, Cancelled=3, Delayed=4).");
        }
    }
}
