using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFleetConsole.Models.Interfaces
{
    public interface ICargoCarrier
    {
        double CapacityKg { get; }
        double CurrentLoadKg { get; }
        bool IsLoadValid(double kg, out string? message);
        void UnloadAll(out string? message);
    }
}
