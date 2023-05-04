using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.Models;
using System.Security.Claims;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FollowController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        public FollowController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }
        [HttpGet]
        public ActionResult Get()
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Follows = _socialMediaDb.Follows
                .Where(x => x.UserFollowedId == UserId)
                .ToList();
            if(Follows == null)
            {
                return NotFound();
            }
            return Ok(Follows);
        }
        [HttpGet("id")]
        public ActionResult Get(int FollowId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Follow = _socialMediaDb.Follows
                .Where(x => x.UserFollowedId == UserId && x.FollowId == FollowId)
                .FirstOrDefault();
            if (Follow == null)
            {
                return NotFound();
            }
            return Ok(Follow);
        }
        [HttpPost]
        public ActionResult Post(int UserId)
        {
            var CurrentUserId = int.Parse(User.FindFirstValue("UserId"));
            if(CurrentUserId == UserId || UserId == null)
            {
                return BadRequest();
            }
            var User1 = _socialMediaDb.Users
                .Where(x => x.UserId == UserId)
                .FirstOrDefault();
            if (User1 == null)
            {
                return BadRequest();
            }
            var Follow = new Follow();
            Follow.UserFollowedId = UserId;
            Follow.FollowedById = CurrentUserId;
            _socialMediaDb.Add(Follow);
            _socialMediaDb.SaveChanges();
            return Ok(Follow);

        }
        [HttpDelete]
        public ActionResult Delete(int UserId)
        {
            var CurrentUserId = int.Parse(User.FindFirstValue("UserId"));
            if (CurrentUserId == UserId || UserId == null)
            {
                return BadRequest();
            }
            var User1 = _socialMediaDb.Users
                .Where(x => x.UserId == UserId)
                .FirstOrDefault();
            if (User1 == null)
            {
                return BadRequest();
            }
            var Follow = _socialMediaDb.Follows
                .Where(x => x.UserFollowedId == UserId && x.FollowedById == CurrentUserId)
                .FirstOrDefault();
            if(Follow == null)
            {
                return BadRequest();
            }
            _socialMediaDb.Follows.Remove(Follow);
            _socialMediaDb.SaveChanges();
            return Ok("Follow removed!");
        }
    }
}
