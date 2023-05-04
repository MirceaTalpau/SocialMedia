using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Like_Post
    {
        public int Like_PostId { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }

        public DateTime CreatedDate { get; set; }
        public Post Post { get; set; }
        public User User { get; set; }
        

        
    }
}
