using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Domain.Entities
{
    public class Booking
    {
        public long Id { get; set; }

        public string UserId { get; set; } = null!;

        public int BookingStatusId { get; set; }

        public DateTime CreatedUtc { get; set; }

        public string ConfirmationCode { get; set; } = null!;

        public int Quantity { get; set; }

    }
}

