using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Domain.Entities
{
    public class Flight
    {
        public int Id { get; set; }
        public int AirlineId { get; set; }
        public string FlightNumber { get; set; } = null!;
        public int OriginAirportId { get; set; }
        public int DestinationAirportId { get; set; }
        public int? DefaultAircraftId { get; set; }
        public bool IsActive { get; set; }
    }
}
