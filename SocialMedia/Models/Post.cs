using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SocialMedia.Models
{
    public class Post
    {
        [Required]
        public int PostId { get; set; }
        [Required]
        public int UserId { get; set; }
        [MinLength(4)]
        public string? PictureURL { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 4)]
        [AllowNull]
        public string? Description { get; set; }
        [Required]
        public bool IsPublic { get; set; }
        [Required]
        public DateTime PostDate { get; set; }
        public User User { get; set; }

    }
}
