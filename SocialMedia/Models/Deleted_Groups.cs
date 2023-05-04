using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Deleted_Groups
    {
        [Key]
        public int Deleted_GroupsId { get; set; }
        [Required] 
        public int GroupId { get; set; }
        [Required]
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

