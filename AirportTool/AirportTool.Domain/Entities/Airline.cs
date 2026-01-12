using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Domain.Entities
{
    public class Airline
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Iatacode { get; set; }
    }
}
