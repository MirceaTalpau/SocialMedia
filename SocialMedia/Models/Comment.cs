using FluentNHibernate.Conventions.Inspections;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialMedia.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        [ForeignKey(nameof(UserId))]
        public int UserId { get; set; }
        [ForeignKey(nameof(PostId))]

        public int PostId { get; set; }

        public string Text { get; set; }
        public DateTime DateCreated { get; set; }
        
        public Post Post { get; set; }

        public User User { get; set; }
    }
}
