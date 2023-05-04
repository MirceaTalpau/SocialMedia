using System.ComponentModel.DataAnnotations;

namespace SocialMedia.DTOs
{
    public class UserDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        [MinLength(10)]
        public string Email { get; set; }
        [Required]
        public string HashPassword { get; set; }
        [Required]
        [MinLength (3)]
        public string FirstName { get; set; }
        [Required]
        [MinLength(3)]
        public string LastName { get; set; }
        [MinLength(3)]
        public string? ProfilePictureURL { get; set; }
        public string? CoverPictureURL { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        [StringLength(10)]
        public string PhoneNumber { get; set; }
        [MinLength(4)]
        public string Country { get; set; }
        [MinLength(4)]
        public string City { get; set; }
        public string Address { get; set; }
    }
}
