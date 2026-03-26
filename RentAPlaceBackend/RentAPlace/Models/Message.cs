using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RentAPlace.API.Models
{
    public class Message
    {
        public int MessageId { get; set; }

        public int PropertyId { get; set; }

        // who sent it
        public int SenderId { get; set; }

        // owner of property / or renter replying
        public int ReceiverId { get; set; }

        [Required]
        [MaxLength(2000)]
        public string Content { get; set; } = string.Empty;

        // null if it's a new message, set if it's a reply
        public int? ParentMessageId { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        // nav properties
        [ForeignKey("PropertyId")]
        public Property? Property { get; set; }

        [ForeignKey("SenderId")]
        public User? Sender { get; set; }

        [ForeignKey("ReceiverId")]
        public User? Receiver { get; set; }
    }
}
