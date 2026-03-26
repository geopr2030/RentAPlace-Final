using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAPlace.API.Models
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        public int PropertyId { get; set; }

        public int RenterId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalPrice { get; set; }

        // Pending, Confirmed, Cancelled
        public string Status { get; set; } = "Pending";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // nav properties
        [ForeignKey("PropertyId")]
        public Property? Property { get; set; }

        [ForeignKey("RenterId")]
        public User? Renter { get; set; }
    }
}
