using System;
using System.Collections.Generic;

namespace AirportTool.Infrastructure.Persistence.Entities;

public partial class Gate
{
    public int Id { get; set; }

    public int AirportId { get; set; }

    public string Code { get; set; } = null!;

    public virtual Airport Airport { get; set; } = null!;

    public virtual ICollection<FlightSchedule> FlightSchedules { get; set; } = new List<FlightSchedule>();
}
