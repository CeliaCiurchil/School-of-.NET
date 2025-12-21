using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Domain.Entities
{
    public class Airport
    {
        public int Id { get; set; }
        public string Iatacode { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string TimeZone { get; set; } = null!;
        public int AddressId { get; set; }
    }
}
