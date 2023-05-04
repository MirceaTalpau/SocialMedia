using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Friendship
    {
        [Key]
        public int FriendshipId { get; set; }
        [Required]
        [ForeignKey("User1")]
        public int User1Id { get; set; }
        [Required]
        [ForeignKey("User2")]
        public int User2Id { get; set; }
        [Required]
        [ForeignKey("FriendRequest")]
        public int FriendRequestId { get; set; }
        public DateTime CreatedAt { get; set; }
        public User User1 { get; set; }
        public User User2 { get; set; }
        public FriendRequest FriendRequest { get; set; }

    }
}
