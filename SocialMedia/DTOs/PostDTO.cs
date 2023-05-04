namespace SocialMedia.DTOs
{
    public class PostDTO
    {
        public int? PostId { get; set; }
        public string? PictureURL { get; set; }
        public string? Description { get; set; }
        public bool IsPublic { get; set; } = true;
        public DateTime PostDate { get; set; }
    }
}

  
