using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.ModelDto.FlightSchedule.Stats
{
    public class UpcomingFlightsDto
    {
        public DateTime Date { get; set; }
        public int Count { get; set; } = 0;
    }
}
