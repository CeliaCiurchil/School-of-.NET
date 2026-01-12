using System;
using System.Collections.Generic;

namespace AirportTool.Infrastructure.Persistence.Entities;

public partial class Airport
{
    public int Id { get; set; }

    public string Iatacode { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string TimeZone { get; set; } = null!;

    public int AddressId { get; set; }

    public virtual Address Address { get; set; } = null!;

    public virtual ICollection<Flight> FlightDestinationAirports { get; set; } = new List<Flight>();

    public virtual ICollection<Flight> FlightOriginAirports { get; set; } = new List<Flight>();

    public virtual ICollection<Gate> Gates { get; set; } = new List<Gate>();
}
