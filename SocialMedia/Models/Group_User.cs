using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Group_User
    {
        [Key]
        public int Group_UserId { get; set; }
        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        [Required]
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        public DateTime DateJoined { get; set; }
        public User User { get; set; }
        public Group Group { get; set; }
    }
}
