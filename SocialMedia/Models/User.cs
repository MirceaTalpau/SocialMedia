using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SocialMedia.Models
{
    public class User
    {
        [Required]
        
        public int UserId { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string HashPassword { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 4)]
        [AllowNull]
        public string? CoverPictureURL { get; set; }
        [StringLength(int.MaxValue, MinimumLength = 4)]
        [AllowNull]
        public string? ProfilePictureURL { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        public DateTime JoinedDate { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public ICollection<Post> Posts { get; set; }
        public ICollection<Like_Post> LikePost { get; set; }
        public ICollection<Like_Comment> LikeComment { get; set; }
    }
}
