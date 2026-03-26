namespace RentAPlace.API.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int ReviewId { get; set; }
        public int PropertyId { get; set; }
        public int ReviewerId { get; set; }
        public string ReviewerName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
