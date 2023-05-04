using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Group
    {
        [Key]
        public int GroupId { get; set; }
        [Required]
        [ForeignKey("Author")]
        public int CreatedById { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsPublic { get; set; }
        public User Author { get; set; }
        public List<User> Members { get; set; } 
        public List<Post> Posts { get; set; } 
    }
}
