using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFleetConsole.Models.Interfaces
{
    public interface INavigable
    {
        Coordinates? CurrentWaypoint { get; }
        void SetWaypoint(Coordinates coordinates);
    }
}
