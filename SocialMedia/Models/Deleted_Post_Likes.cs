namespace SocialMedia.Models
{
    public class Deleted_Post_Likes
    {
        public int Deleted_Post_LikesId { get; set; }
        public int Like_PostId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
