using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.ModelDto.Booking
{
    public class BaseBookingDto
    {
        public string UserId { get; set; } = null!;

        public int BookingStatusId { get; set; }

        public DateTime CreatedUtc { get; set; }

        public string ConfirmationCode { get; set; } = null!;

        public int Quantity { get; set; }
    }
}

