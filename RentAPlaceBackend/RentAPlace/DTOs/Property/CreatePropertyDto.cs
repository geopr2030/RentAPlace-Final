using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs.Property
{
    public class CreatePropertyDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public string Location { get; set; } = string.Empty;

        // Flat, Villa, Apartment
        [Required]
        public string PropertyType { get; set; } = string.Empty;

        [Required]
        [Range(1, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        public decimal PricePerNight { get; set; }

        // e.g. "Pool,Garden,Beach View"
        public string? Features { get; set; }
    }
}
