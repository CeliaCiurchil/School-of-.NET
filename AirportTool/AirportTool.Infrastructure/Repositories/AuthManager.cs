using AirportTool.Application.Contracts;
using AirportTool.Application.ModelDto.Users;
using AirportTool.Infrastructure.Persistence.Identity;
using AutoMapper;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AirportTool.Infrastructure.Repositories
{
    public class AuthManager : IAuthManager
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;

        private ApiUser _user;

        public AuthManager(IMapper mapper, UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<AuthResponseDto?> Login(ApiUserLoginDto userLoginDto) //api are stateless, they dont know when they are being accessed
        {
            _user = await _userManager.FindByEmailAsync(userLoginDto.Email);
            bool isValidUser = await _userManager.CheckPasswordAsync(_user, userLoginDto.Password);

            if (_user == null || isValidUser == false)
            {
                return null;
            }

            var token = await GenerateToken();
            
            return new AuthResponseDto
            {
                UserId = _user.Id,
                Token = token
            };
        }

        public async Task<RegistrationResponseDto> RegisterUser(ApiUserDto userDto)
        {
            _user = _mapper.Map<ApiUser>(userDto);
            _user.UserName = userDto.Email;

            var result = await _userManager.CreateAsync(_user, userDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(_user, "Client");
            }


            return new RegistrationResponseDto
            {
                Succeeded = result.Succeeded,
                Errors = result.Errors.Select(e => new RegistrationErrorDto
                {
                    Code = e.Code,
                    Description = e.Description
                })
            };
        }

        private async Task<string> GenerateToken()
        {
            // Create a symmetric security key from the JWT secret key stored in configuration
            // This key will be used to sign the token to ensure it hasn't been tampered with
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));

            // Create signing credentials using the security key and HMAC SHA256 algorithm
            // This determines how the token will be cryptographically signed
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Retrieve all roles assigned to the user from the database
            // Returns a collection of role names (e.g., "Admin", "User")
            var roles = await _userManager.GetRolesAsync(_user);

            // Transform each role name into a Claim object with ClaimTypes.Role as the claim type
            // Claims are key-value pairs that describe the user's identity and permissions
            var rolesClaims = roles.Select(x => new Claim(ClaimTypes.Role, x)).ToList();

            // Retrieve any additional custom claims that were previously assigned to the user
            // These are stored in the AspNetUserClaims table
            var userClaims = await _userManager.GetClaimsAsync(_user);

            // Create a list of standard JWT claims about the user
            var claims = new List<Claim>
            {
                // Sub (Subject): The primary identifier for the user - their email
                new Claim(JwtRegisteredClaimNames.Sub, _user.Email),
                
                // Jti (JWT ID): A unique identifier for this specific token to prevent replay attacks
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                
                // Email: The user's email address for easy access in the token
                new Claim(JwtRegisteredClaimNames.Email, _user.Email),
                
                // uid: Custom claim containing the user's unique ID from the database
                new Claim("uid", _user.Id),
            }
            .Union(rolesClaims)  // Combine the standard claims with the role claims
            .Union(userClaims);  // Combine all previous claims with custom user claims

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtSettings:Issuer"],
                audience: _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JwtSettings:DurationInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
