using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.ModelDto.Ticket
{
    public class TicketReadDto
    {
        public long Id { get; set; }

        public long BookingId { get; set; }

        public int FlightScheduleId { get; set; }

        public string FareClass { get; set; } = null!;

        public decimal BasePrice { get; set; }

        public decimal Taxes { get; set; }

        public decimal TotalPrice { get; set; }

        public string Currency { get; set; } = null!;

        public bool IsRefundable { get; set; }

        public string SeatNumber { get; set; } = null!;

        public string PassengerFullName { get; set; } = null!;

        public string PassengerEmail { get; set; } = null!;
    }
}
