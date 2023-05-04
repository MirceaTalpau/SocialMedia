using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Deleted_Comments
    {
        [Key]
        public int Deleted_CommentsId { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
