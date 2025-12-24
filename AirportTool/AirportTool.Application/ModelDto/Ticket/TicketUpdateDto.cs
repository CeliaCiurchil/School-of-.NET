using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.ModelDto.Ticket
{
    public class TicketUpdateDto
    {
        public long BookingId { get; set; }

        public int FlightScheduleId { get; set; }

        public string FareClass { get; set; } = null!;

        public string SeatNumber { get; set; } = null!;

        public string PassengerFullName { get; set; } = null!;

        public string PassengerEmail { get; set; } = null!;
    }
}
