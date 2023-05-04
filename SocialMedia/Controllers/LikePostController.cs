using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using System.Diagnostics;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LikePostController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        public LikePostController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }

        [HttpGet]
        public ActionResult Get(int PostId) 
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Post = _socialMediaDb.Posts.Find(PostId);
            var PostLikes = _socialMediaDb.LikePosts
                .Where(p => p.PostId == PostId)
                .ToList();
            if (UserId == null || Post == null || PostLikes == null)
            {
                return BadRequest("Bad request");
            }
            if(Post.UserId == UserId || Post.IsPublic == true)
            {
                return Ok(PostLikes);
            }
            return BadRequest("Ceva nu e bine");



        }
        [HttpPost]
        public ActionResult Add(int PostId) 
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Post = _socialMediaDb.Posts.Find(PostId);
            var PostLikes = _socialMediaDb.LikePosts
                .Where(p => p.PostId == PostId && p.UserId == UserId)
                .FirstOrDefault();
            if (UserId == null || Post == null || PostLikes != null)
            {
                return BadRequest("Bad request");
            }
            var PostLike = new Like_Post();
            PostLike.UserId = UserId;
            PostLike.PostId = PostId;
            PostLike.CreatedDate = DateTime.Now;
            _socialMediaDb.LikePosts.Add(PostLike);
            _socialMediaDb.SaveChanges();
            return Ok(PostLike);

        }
        [HttpDelete]
        public ActionResult Delete(int PostId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Post = _socialMediaDb.Posts.Find(PostId);
            var PostLikes = _socialMediaDb.LikePosts
                .Where(p => p.PostId == PostId && p.UserId == UserId)
                .FirstOrDefault();
            if (UserId == null || Post == null || PostLikes == null)
            {
                return BadRequest("Bad request");
            }
            _socialMediaDb.Remove(PostLikes);
            _socialMediaDb.SaveChanges();
            return Ok();
        }


    }
}
