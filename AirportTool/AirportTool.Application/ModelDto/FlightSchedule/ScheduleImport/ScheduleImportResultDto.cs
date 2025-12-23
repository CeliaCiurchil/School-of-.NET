using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.ModelDto.FlightSchedule.ScheduleImport
{
    public class ScheduleImportResultDto
    {
        public int Total { get; set; }
        public int Created { get; set; }
        public int Updated { get; set; }
        public List<ScheduleImportErrorDto> Errors { get; set; } = new();
    }

    public class ScheduleImportErrorDto
    {
        public int Row { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
