using Microsoft.EntityFrameworkCore;

namespace SocialMedia.Models
{
    public class SocialMediaDb:DbContext
    {
        public SocialMediaDb(DbContextOptions<SocialMediaDb> options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like_Post> LikePosts { get; set; }
        public DbSet<Like_Comment> LikeComments { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Friendship> Friendships { get; set; }
        public DbSet<FriendRequest> FriendRequests { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Group_User> GroupUsers { get; set; }
        public DbSet<Group_Post> GroupPosts { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Deleted_Groups> Deleted_Groups { get; set; }
        public DbSet<Deleted_Comments> Deleted_Comments { get; set; }
        public DbSet<Deleted_Posts> Deleted_Posts { get; set; }
        public DbSet<Deleted_Comment_Likes> Deleted_Comment_Likes { get; set; }
        public DbSet<Deleted_Post_Likes> Deleted_Post_Likes { get; set; }

    }
}
