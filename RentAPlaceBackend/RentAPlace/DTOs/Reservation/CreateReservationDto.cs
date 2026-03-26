using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs.Reservation
{
    public class CreateReservationDto
    {
        [Required]
        public int PropertyId { get; set; }

        [Required]
        public DateTime CheckInDate { get; set; }

        [Required]
        public DateTime CheckOutDate { get; set; }
    }
}
