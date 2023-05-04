using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Models;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        private CommentController commentController;
        public PostController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult Get(int Id) 
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var post = _socialMediaDb.Posts.Find(Id);
            if(post == null || post != null && post.UserId != UserId)
            {
                return NotFound("The post doesn't exist!");
            }
            return Ok(post);

        }
        
        [Authorize]
        [HttpPost]
        public ActionResult AddPost(PostDTO payload)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            if(payload == null || payload.PictureURL == null && payload.Description == null || payload.PostId != null || payload.PostDate == null)
            {
                return BadRequest("There was an error adding the post!");
            }
            var post = new Post();
            post.PictureURL = payload.PictureURL;
            post.Description = payload.Description;
            post.PostDate = payload.PostDate;
            post.IsPublic = payload.IsPublic;
            post.UserId=UserId;
            
            _socialMediaDb.Add(post);
            _socialMediaDb.SaveChanges();
            return Ok("Post was added succesfully!");
        } 

        
        [Authorize]
        [HttpPut]
        public ActionResult EditPost(PostDTO payload)
        {
            var UserId = User.FindFirstValue("UserId");
            if (payload != null && payload.PostId != null)
            {
                var post = _socialMediaDb.Posts.Find(payload.PostId);
                if(post != null && post.UserId == int.Parse(UserId))
                {
                    if(payload.PictureURL == null && payload.Description == null)
                    {
                        return BadRequest("There was an error updating the post!");
                    }
                    post.PictureURL = payload.PictureURL;
                    post.Description = payload.Description;
                    post.PostDate = payload.PostDate;
                    post.IsPublic = payload.IsPublic;
                    _socialMediaDb.Update(post);
                    _socialMediaDb.SaveChanges();
                    return Ok("Post succesfully updated!");
                }
            }
            return BadRequest("There was an error updating the post!");
        }
        [Authorize]
        [HttpDelete]
        public ActionResult Delete(int PostId)
        {
            var userId = User.FindFirstValue("UserId");
            if(PostId == null)
            {
                return NotFound();
            }
            var Post = _socialMediaDb.Posts.Find(PostId);
            if (Post != null && Post.UserId == int.Parse(userId))
            {
                var Comments = _socialMediaDb.Comments
                    .Where(x => x.PostId == PostId)
                    .ToList();
                if (Comments.Any())
                {
                    foreach (var c in Comments)
                    {
                        commentController.DeleteMultipleComments(c.CommentId);
                    }
                    _socialMediaDb.RemoveRange(Comments);
                }
                var LikesPost = _socialMediaDb.LikePosts
                    .Where(x => x.PostId == PostId)
                    .ToList();
                var DeletedLikesPost = new Deleted_Post_Likes();
                if (LikesPost.Any())
                {
                    foreach (var l in LikesPost)
                    {
                        DeletedLikesPost.UserId = l.UserId;
                        DeletedLikesPost.PostId = l.PostId;
                        DeletedLikesPost.Like_PostId = l.Like_PostId;
                        _socialMediaDb.Deleted_Post_Likes.Add(DeletedLikesPost);
                    }
                    _socialMediaDb.RemoveRange(LikesPost);
                }
                var DeletePost = new Deleted_Posts();
                DeletePost.PostId = Post.PostId;
                DeletePost.PostDate = Post.PostDate;
                DeletePost.PictureURL = Post.PictureURL;
                DeletePost.Description = Post.Description;
                DeletePost.IsPublic = Post.IsPublic;
                DeletePost.UserId = Post.UserId;
                _socialMediaDb.Add(DeletePost);
                _socialMediaDb.Remove(Post);
                _socialMediaDb.SaveChanges();
                return Ok("Post deleted succesfully!");
            }
            else
            {
                return NotFound("Post does not exist!");
            }
        }
        public void DeleteMultiplePosts(int PostId)
        {
            if (PostId == null)
            {
                return;
            }
            var Post = _socialMediaDb.Posts.Find(PostId);
            if (Post != null)
            {
                var Comments = _socialMediaDb.Comments
                    .Where(x => x.PostId == PostId)
                    .ToList();
                if (Comments.Any())
                {
                    foreach (var c in Comments)
                    {
                        commentController.DeleteMultipleComments(c.CommentId);
                    }
                    _socialMediaDb.RemoveRange(Comments);
                }
                var LikesPost = _socialMediaDb.LikePosts
                    .Where(x => x.PostId == PostId)
                    .ToList();
                var DeletedLikesPost = new Deleted_Post_Likes();
                if (LikesPost.Any())
                {
                    foreach (var l in LikesPost)
                    {
                        DeletedLikesPost.UserId = l.UserId;
                        DeletedLikesPost.PostId = l.PostId;
                        DeletedLikesPost.Like_PostId = l.Like_PostId;
                        _socialMediaDb.Deleted_Post_Likes.Add(DeletedLikesPost);
                    }
                    _socialMediaDb.RemoveRange(LikesPost);
                }
                var DeletePost = new Deleted_Posts();
                DeletePost.PostId = Post.PostId;
                DeletePost.PostDate = Post.PostDate;
                DeletePost.PictureURL = Post.PictureURL;
                DeletePost.Description = Post.Description;
                DeletePost.IsPublic = Post.IsPublic;
                DeletePost.UserId = Post.UserId;
                _socialMediaDb.Add(DeletePost);
                return ;
            }
            else
            {
                return;
            }
        }
    }
}
