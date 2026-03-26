using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        // Role: Admin, Owner, Renter
        [Required]
        public string Role { get; set; } = "Renter";

        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // nav properties
        public ICollection<Property>? Properties { get; set; }
        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Message>? SentMessages { get; set; }
    }
}
