using HotelListing.API.Contracts;
using HotelListing.API.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;

namespace HotelListing.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAuthManager _authManager;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IAuthManager authManager, ILogger<AccountController> logger)
        {
            _authManager = authManager;
            _logger = logger;
        }

        [HttpPost] //multiple posts
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Login([FromBody] ApiUserLoginDto userDto)
        {
            _logger.LogInformation($"Login attempt for {userDto.Email}");
            try
            {
                var authResponse = await _authManager.Login(userDto);
                if (authResponse == null)
                {
                    return Unauthorized(); //403- authorised but you dont have the role, 401- unauthorized ? unauthenticated
                }
                return Ok(authResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Problem in {nameof(Login)}");
                return Problem($"Problem in {nameof(Login)} contact support", statusCode: 500);
            }
        }//we dont want to always call login when i need something, so we need jwt token
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> Register([FromBody] ApiUserDto userDto)
        {
            _logger.LogInformation($"Registation attempt for {userDto.Email}");
            try
            {
                var errors = await _authManager.RegisterUser(userDto);
                if (errors.Any())
                {
                    foreach (var error in errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description); //what handles the error, modelstate carries the text of error when bad request for example
                    }
                    return BadRequest(ModelState);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Somntheing went wrong in {nameof(Register)}");
                return Problem($"Somntheing went wrong in {nameof(Register)}", statusCode: 500);
            }
        }
        [HttpPost]
        [Route("refreshtoken")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> RefreshToken([FromBody] AuthResponseDto request)
        {
            var authResponse = await _authManager.VerifyRefreshToken(request);
            if(authResponse == null)
                { return Unauthorized(); }
            return Ok(authResponse);
        }
    }
}
