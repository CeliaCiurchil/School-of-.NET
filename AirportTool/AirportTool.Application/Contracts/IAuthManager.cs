using AirportTool.Application.ModelDto.Users;
using HotelListing.API.Models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Application.Contracts
{
    public interface IAuthManager
    {
        Task<RegistrationResponseDto> RegisterUser(ApiUserDto userDto);
        Task<AuthResponseDto?> Login(ApiUserLoginDto userLoginDto);
    }
}
