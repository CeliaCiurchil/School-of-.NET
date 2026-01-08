using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport
{
    public class ScheduleImportErrorDto
    {
        public int Row { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
