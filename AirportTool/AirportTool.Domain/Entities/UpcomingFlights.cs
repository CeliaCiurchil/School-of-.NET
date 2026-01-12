using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Domain.Entities
{
    public class UpcomingFlights
    {
        public DateTime Date { get; set; }
        public int Count { get; set; } = 0;
    }
}
