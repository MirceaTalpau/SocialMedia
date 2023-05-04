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
    public class FriendshipController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        public FriendshipController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }
        [HttpGet]
        public ActionResult Get()
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Friendships = _socialMediaDb.Friendships
                .Where(x => x.User1Id == UserId || x.User2Id == UserId)
                .ToList();
            if(Friendships == null || Friendships.Count == 0)
            {
                return NotFound("I'm sorry,you have no friends!");
            }
            return Ok(Friendships);
        }
        [HttpDelete] 
        public ActionResult Delete(int FriendshipId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var Friendship = _socialMediaDb.Friendships
                .Where(x => x.FriendshipId == FriendshipId && (x.User1Id == UserId || x.User2Id == UserId) )
                .FirstOrDefault();
            if(Friendship == null)
            {
                return NotFound();
            }
            var FriendRequest = _socialMediaDb.FriendRequests
                .Where(x => x.FriendRequestId == Friendship.FriendRequestId)
                .FirstOrDefault();
            if(FriendRequest == null)
            {
                return BadRequest();
            }
            _socialMediaDb.FriendRequests.Remove(FriendRequest);
            _socialMediaDb.Friendships.Remove(Friendship);
            _socialMediaDb.SaveChanges();
            return Ok("Friendship deleted succesfully!");
            
        }
    }
}
