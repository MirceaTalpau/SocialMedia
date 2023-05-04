using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Follow
    {
        [Key]
        public int FollowId { get; set; }
        [Required]
        [ForeignKey("UserFollowed")]
        public int UserFollowedId { get; set; }
        [Required]
        [ForeignKey("FollowedBy")]
        public int FollowedById { get; set; }

        public User UserFollowed { get; set; }
        public User FollowedBy { get; set; }

    }
}
