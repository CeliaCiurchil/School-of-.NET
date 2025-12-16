using System;
using System.Collections.Generic;

namespace AirportTool.Infrastructure.Persistence.Entities;

public partial class Booking
{
    public long Id { get; set; }

    public int UserId { get; set; }

    public int BookingStatusId { get; set; }

    public DateTime CreatedUtc { get; set; }

    public string ConfirmationCode { get; set; } = null!;

    public int Quantity { get; set; }

    public virtual BookingStatus BookingStatus { get; set; } = null!;

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual User User { get; set; } = null!;
}
