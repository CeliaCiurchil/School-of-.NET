using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.ModelDto.Users
{
    public class RegistrationResponseDto
    {
        public bool Succeeded { get; set; }
        public IEnumerable<RegistrationErrorDto> Errors { get; set; } = new List<RegistrationErrorDto>();
    }
    public class RegistrationErrorDto
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
