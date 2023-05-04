using FluentNHibernate.Conventions.Helpers;
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
    [Authorize]
    public class CommentController : ControllerBase
    {
        private readonly SocialMediaDb _socialMedia;
        public CommentController(SocialMediaDb socialMedia)
        {
            _socialMedia = socialMedia;
        }
        [HttpGet]
        public ActionResult Get(int PostId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            if (UserId == null || PostId == null)
            {
                return BadRequest();
            }
            var Post = _socialMedia.Posts
                        .Where(x => x.PostId == PostId)
                        .FirstOrDefault();
            if(Post == null)
            {
                return NotFound();
            }
            if (Post.UserId != UserId && Post.IsPublic == false)
            {
                return BadRequest();
            }
            var Comments = _socialMedia.Comments
                                    .Where(c => c.PostId == PostId)
                                    .ToList();
            if (Comments == null)
            {
                return BadRequest();
            }
            return Ok(Comments);

        }

        [HttpPost]
        public ActionResult Post([FromBody] CommentDTO payload) 
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Post = _socialMedia.Posts.Find(payload.PostId);
            if(UserId == null || payload == null || Post == null)
            {
                return BadRequest();
            }
            var Comment = new Comment();
            Comment.UserId = UserId;
            Comment.Text = payload.Text;
            Comment.PostId = payload.PostId;
            Comment.DateCreated = DateTime.Now;
            _socialMedia.Comments.Add(Comment);
            _socialMedia.SaveChanges();
            return Ok("Comment succesfully added!");
        }
        
        [HttpDelete]
        public ActionResult Delete(int CommentId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            if(CommentId != null)
            {
                var Comment = _socialMedia.Comments.Find(CommentId);
                if (Comment == null || Comment.UserId != UserId)
                {
                    return BadRequest();
                }
                var DeletedLikes = _socialMedia.LikeComments
                    .Where(x => x.CommentId == CommentId)
                    .ToList();
                if(DeletedLikes.Any())
                {
                    var Deleted = new Deleted_Comment_Likes();
                    foreach(var d in DeletedLikes)
                    {
                        Deleted.CreatedDate = d.CreatedDate;
                        Deleted.UserId = d.UserId;
                        Deleted.CommentId = d.CommentId;
                        Deleted.Like_CommentId = d.Like_CommentId;
                        _socialMedia.Deleted_Comment_Likes.Add(Deleted);
                    }
                    _socialMedia.RemoveRange(DeletedLikes);
                }
                var DeletedComment = new Deleted_Comments();
                DeletedComment.CommentId = Comment.CommentId;
                DeletedComment.Text = Comment.Text;
                DeletedComment.UserId = Comment.UserId;
                DeletedComment.DateCreated = Comment.DateCreated;
                DeletedComment.PostId = Comment.PostId;
                _socialMedia.Deleted_Comments.Add(DeletedComment);
                _socialMedia.Remove(Comment);
                _socialMedia.SaveChanges();
                return Ok();
            }
            return BadRequest();

        }
        public void DeleteMultipleComments(int CommentId)
        {
            if (CommentId != null)
            {
                var Comment = _socialMedia.Comments.Find(CommentId);
                if (Comment == null)
                {
                    return;
                }
                var DeletedLikes = _socialMedia.LikeComments
                    .Where(x => x.CommentId == CommentId)
                    .ToList();
                if (DeletedLikes.Any())
                {
                    var Deleted = new Deleted_Comment_Likes();
                    foreach (var d in DeletedLikes)
                    {
                        Deleted.CreatedDate = d.CreatedDate;
                        Deleted.UserId = d.UserId;
                        Deleted.CommentId = d.CommentId;
                        Deleted.Like_CommentId = d.Like_CommentId;
                        _socialMedia.Deleted_Comment_Likes.Add(Deleted);
                    }
                    _socialMedia.RemoveRange(DeletedLikes);
                }
                var DeletedComment = new Deleted_Comments();
                DeletedComment.CommentId = Comment.CommentId;
                DeletedComment.Text = Comment.Text;
                DeletedComment.UserId = Comment.UserId;
                DeletedComment.DateCreated = Comment.DateCreated;
                DeletedComment.PostId = Comment.PostId;
                _socialMedia.Deleted_Comments.Add(DeletedComment);
                
                return;
            }
            return;
        }
    }
}
