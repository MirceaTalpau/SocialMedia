using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Group_Post
    {
        [Key]
        public int Group_PostId { get; set; }
        [Required]
        [ForeignKey("Post")]
        public int PostId { get; set; }
        [Required]
        [ForeignKey("Group")]
        public int GroupId { get; set; }
        
        public Post Post { get; set; }
        public Group Group { get; set; }
    }
}
