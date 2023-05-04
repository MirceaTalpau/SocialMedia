using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SocialMedia.Models
{
    public class Deleted_Posts
    {
        public int Deleted_PostsId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string? PictureURL { get; set; }
        
        public string? Description { get; set; }
        public bool IsPublic { get; set; }
        public DateTime PostDate { get; set; }
    }
}
