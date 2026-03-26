using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs.Auth
{
    public class RegisterDto
    {
        [Required]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        // Renter or Owner — Admin created manually
        public string Role { get; set; } = "Renter";

        public string? PhoneNumber { get; set; }
    }
}
