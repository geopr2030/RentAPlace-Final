using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAPlace.API.Models
{
    // optional feature — review after a stay
    public class Review
    {
        public int ReviewId { get; set; }

        public int PropertyId { get; set; }

        public int ReviewerId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey("PropertyId")]
        public Property? Property { get; set; }

        [ForeignKey("ReviewerId")]
        public User? Reviewer { get; set; }
    }
}
