using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using System.ComponentModel.Design;
using System.Security.Claims;
using System.Xml.Linq;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikeCommentController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        public LikeCommentController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }
        [HttpGet]
        public ActionResult Get(int CommentId) 
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Comment = _socialMediaDb.Comments.Find(CommentId);
            if(Comment == null)
            {
                return NotFound();
            }
            var Post = _socialMediaDb.Posts.Find(Comment.PostId);
            if( UserId == null || Post == null || Post.IsPublic == false && UserId != Post.UserId )
            {
                return BadRequest();
            }
            var LikeComment = _socialMediaDb.LikeComments
                                .Where(x =>  x.CommentId == CommentId)
                                .ToList();
            return Ok(LikeComment);
        }
        
        [HttpPost]
        public ActionResult Post(int CommentId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Comment = _socialMediaDb.Comments.Find(CommentId);
            var Post = _socialMediaDb.Posts.Find(Comment.PostId);
            var Duplicate = _socialMediaDb.LikeComments
                             .Where(x => x.UserId == UserId && x.CommentId == CommentId)
                             .FirstOrDefault();
            if (Duplicate != null || Comment == null || Post == null || UserId == null || Post.IsPublic == false && UserId != Post.UserId)
            {
                return BadRequest();
            }

            var LikeComment = new Like_Comment();
            LikeComment.UserId = UserId;
            LikeComment.CommentId = CommentId;
            LikeComment.CreatedDate = DateTime.Now;
            _socialMediaDb.LikeComments.Add(LikeComment);
            _socialMediaDb.SaveChanges();
            return Ok(LikeComment);
        }
        [HttpDelete]
        public ActionResult Delete(int CommentId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var LikeComment = _socialMediaDb.LikeComments
                                .Where(x => x.CommentId == CommentId && x.UserId == UserId)
                                .FirstOrDefault();
            if (LikeComment != null)
            {
                _socialMediaDb.Remove(LikeComment);
                _socialMediaDb.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
    }
}
