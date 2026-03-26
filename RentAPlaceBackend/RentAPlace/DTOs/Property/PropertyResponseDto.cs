namespace RentAPlace.API.DTOs.Property
{
    public class PropertyResponseDto
    {
        public int PropertyId { get; set; }
        public int OwnerId { get; set; }
        public string OwnerName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Location { get; set; } = string.Empty;
        public string PropertyType { get; set; } = string.Empty;
        public decimal PricePerNight { get; set; }
        public List<string> Features { get; set; } = new();
        public List<string> ImageUrls { get; set; } = new();
        public DateTime CreatedDate { get; set; }
    }
}
