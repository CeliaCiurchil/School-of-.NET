using System.ComponentModel.DataAnnotations;

namespace HotelListing.API.Models.Users
{
    public class ApiUserDto : ApiUserLoginDto
    {
        [Required]
        public string FirstName { get; set; } = string.Empty;
        [Required]
        public string LastName { get; set; } = string.Empty;

    }
}
