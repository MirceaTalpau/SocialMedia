namespace SocialMedia.Models
{
    public class Like_Comment
    {
        public int Like_CommentId { get; set; }
        public int UserId { get; set; }
        public int CommentId { get; set; }
        public DateTime CreatedDate { get; set; }

        public Comment Comment { get; set; }
        public User User { get; set; }
    }
}
