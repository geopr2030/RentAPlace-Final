using System.ComponentModel.DataAnnotations;

namespace RentAPlace.API.DTOs.Message
{
    public class SendMessageDto
    {
        [Required]
        public int PropertyId { get; set; }

        [Required]
        public int ReceiverId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; } = string.Empty;

        // for replies
        public int? ParentMessageId { get; set; }
    }
}
