using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAPlace.API.Models
{
    public class Property
    {
        public int PropertyId { get; set; }

        // owner who created this listing
        public int OwnerId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;

        [MaxLength(2000)]
        public string? Description { get; set; }

        [Required]
        [MaxLength(300)]
        public string Location { get; set; } = string.Empty;

        // Flat, Villa, Apartment
        [Required]
        public string PropertyType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal PricePerNight { get; set; }

        // comma-separated: "Pool,Garden,Beach View"
        public string? Features { get; set; }

        // comma-separated file paths
        public string? ImageUrls { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // nav properties
        [ForeignKey("OwnerId")]
        public User? Owner { get; set; }

        public ICollection<Reservation>? Reservations { get; set; }
        public ICollection<Message>? Messages { get; set; }
        public ICollection<Review>? Reviews { get; set; }
    }
}
