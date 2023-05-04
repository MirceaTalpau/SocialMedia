using System.ComponentModel.DataAnnotations;

namespace SocialMedia.Models
{
    public class Deleted_Comment_Likes
    {
        [Key]
        public int Deleted_Comment_likesId { get; set; }
        public int Like_CommentId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
