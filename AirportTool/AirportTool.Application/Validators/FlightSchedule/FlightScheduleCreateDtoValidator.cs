using AirportTool.Application.ModelDto.FlightSchedule;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Validators.FlightSchedule
{
    public class FlightScheduleCreateDtoValidator : AbstractValidator<FlightScheduleCreateDto>
    {
        public FlightScheduleCreateDtoValidator()
        {
            Include(new BaseFlightScheduleValidator());
        }
    }
}
