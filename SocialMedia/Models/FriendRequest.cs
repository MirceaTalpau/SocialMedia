using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class FriendRequest
    {
        [Key]
        public int FriendRequestId { get; set; }
        [Required]
        [ForeignKey("SentByUser")]
        public int SentById { get; set; }
        [Required]
        [ForeignKey("ReceivedByUser")]
        public int ReceivedById { get; set; }
        public bool IsAccepted { get; set; } = false;
        public DateTime SentAt { get; set; }
        public User SentByUser { get; set; }
        public User ReceivedByUser { get; set; }
    }
}
