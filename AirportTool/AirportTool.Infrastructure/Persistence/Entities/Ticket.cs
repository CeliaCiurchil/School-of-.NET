using System;
using System.Collections.Generic;

namespace AirportTool.Infrastructure.Persistence.Entities;

public partial class Ticket
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

    public virtual Booking Booking { get; set; } = null!;

    public virtual FlightSchedule FlightSchedule { get; set; } = null!;
}
