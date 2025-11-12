using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DroneFleetConsole.Models.Interfaces
{
    public interface IFlightControl
    {
        void TakeOff();
        void Land();
    }
}
