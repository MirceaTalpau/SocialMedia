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
    public class FriendRequestController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        public FriendRequestController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }

        [HttpGet]
        public ActionResult Get()
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var FriendRequest = _socialMediaDb.FriendRequests
                .Where(x => x.ReceivedById == UserId)
                .ToList();
            if(FriendRequest == null)
            {
                return NoContent();
            }
            return Ok(FriendRequest);
        }
        [HttpGet("id")]
        public ActionResult Get(int id)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            var FriendRequest = _socialMediaDb.FriendRequests
                .Where(x => x.FriendRequestId == id)
                .FirstOrDefault();
            if (FriendRequest == null)
            {
                return NoContent();
            }
            return Ok(FriendRequest);
        }
        [HttpPost("send")]
        public ActionResult Post(int SendToId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            if(SendToId == null)
            {
                return BadRequest("The id cannot be null!");
            }
            var CheckUserExists = _socialMediaDb.Users
                .Where(x => x.UserId == SendToId)
                .FirstOrDefault();
            if (CheckUserExists == null)
            {
                return NotFound("User doesn't exists!");
            }
            var CheckDuplicate = _socialMediaDb.FriendRequests
                .Where(x => x.ReceivedById == SendToId && x.SentById == UserId || x.ReceivedById == UserId && x.SentById == SendToId)
                .FirstOrDefault();
            if(CheckDuplicate != null)
            {
                return BadRequest("A friend request already exists!");
            }
            var FriendRequest = new FriendRequest();
            FriendRequest.SentById = UserId;
            FriendRequest.ReceivedById = SendToId;
            FriendRequest.IsAccepted = false;
            FriendRequest.SentAt = DateTime.Now;
            _socialMediaDb.FriendRequests.Add(FriendRequest);
            _socialMediaDb.SaveChanges();
            return Ok(FriendRequest);
        }
        [HttpPost("respond")]
        public ActionResult Respond(bool Response,int FriendRequestId)
        {
            var UserId = int.Parse(User.FindFirstValue("UserId"));
            if(Response == null || FriendRequestId == null) 
            {
                return BadRequest("The Response or Id cannot be null!");
            }
            var CheckValidity = _socialMediaDb.FriendRequests
                .Where(x => x.ReceivedById == UserId && x.FriendRequestId == FriendRequestId)
                .FirstOrDefault();
            if(CheckValidity == null)
            {
                return BadRequest("The friend request doesn't exist!");
            }
            if(CheckValidity != null && Response == true)
            {
                var FriendId = CheckValidity.SentById;
                CheckValidity.IsAccepted = Response;
                var Friendship = new Friendship();
                Friendship.User1Id = UserId;
                Friendship.User2Id = FriendId;
                Friendship.CreatedAt = DateTime.Now;
                Friendship.FriendRequestId = FriendRequestId;
                _socialMediaDb.Friendships.Add(Friendship);
                _socialMediaDb.SaveChanges();
                return Ok(Friendship);
            }
            if(CheckValidity != null && Response == false)
            {
                _socialMediaDb.FriendRequests.Remove(CheckValidity);
                _socialMediaDb.SaveChanges();
                return Ok("Friend request declined!");
            }
            return BadRequest();

        }

    }
}
