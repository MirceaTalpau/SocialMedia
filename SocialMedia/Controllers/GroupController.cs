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
    public class GroupController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        private PostController _postController;
        public GroupController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }

        [HttpGet]
        public ActionResult Get(int GroupId)
        {
            var CurrentUser = int.Parse(User.FindFirstValue("UserId"));
            var Group = _socialMediaDb.Groups
                .Where(x => x.GroupId == GroupId)
                .FirstOrDefault();
            if(Group == null)
            {
                return NotFound();
            }
            if(Group.IsPublic == false && CurrentUser != Group.CreatedById)
            {
                return BadRequest();
            }
            var CheckMember = _socialMediaDb.GroupUsers
                .Where(x => x.UserId == CurrentUser && x.GroupId == GroupId) 
                .FirstOrDefault();
            if(CheckMember == null && Group.IsPublic == false)
            {
                return BadRequest();
            }
            return Ok(Group);
        }

        [HttpPost]
        public ActionResult Post([FromBody]GroupDTO payload )
        {
            var CurrentUser = int.Parse(User.FindFirstValue("UserId"));
            if(payload == null)
            {
                return BadRequest();
            }
            var CheckName = _socialMediaDb.Groups
                .Where(x => x.Name == payload.Name)
                .FirstOrDefault();
            if(CheckName != null)
            {
                return BadRequest();
            }
            var Group = new Group();
            Group.CreatedById = CurrentUser;
            Group.Name = payload.Name;
            Group.Description = payload.Description;
            Group.CreatedDate = DateTime.Now;
            if(payload.IsPublic == null)
            {
                Group.IsPublic = true;
            }
            else
            {
                Group.IsPublic = (bool)payload.IsPublic;
            }
            _socialMediaDb.Add(Group);
            _socialMediaDb.SaveChanges();
            return Ok(Group);
        }
        [HttpPut]
        public ActionResult Edit([FromBody]Group payload )
        {
            var CurrentUser = int.Parse(User.FindFirstValue("UserId"));
            if (payload == null || CurrentUser != payload.CreatedById)
            {
                return BadRequest();
            }
            var Group = _socialMediaDb.Groups
                .Where(x => x.GroupId == payload.GroupId)
                .FirstOrDefault();
            if( Group == null )
            {
                return BadRequest();
            }
            Group.Description = payload.Description;
            Group.Name = payload.Name;
            Group.IsPublic = payload.IsPublic;
            _socialMediaDb.Update(Group);
            _socialMediaDb.SaveChanges();
            return Ok(Group);
        }
        [HttpDelete]
        public ActionResult Delete(int GroupId)
        {
            var CurrentUser = int.Parse(User.FindFirstValue("UserId"));
            if(GroupId == null)
            {
                return NotFound();
            }
            var Group = _socialMediaDb.Groups
                .Where(x => x.GroupId == GroupId)
                .FirstOrDefault();
            if( Group == null || Group.CreatedById != CurrentUser )
            {
                return BadRequest();
            }
            var GroupUsers = _socialMediaDb.GroupUsers
                .Where(x => x.GroupId == GroupId)
                .ToList();
            if(GroupUsers.Any())
            {
                _socialMediaDb.GroupUsers.RemoveRange(GroupUsers);
            }
            var GroupPosts = _socialMediaDb.GroupPosts
                .Where(x => x.GroupId == GroupId)
                .ToList();
            if( GroupPosts.Any())
            {
                var DeletedPosts = new Post();
                foreach(var post in GroupPosts)
                {
                    DeletedPosts = _socialMediaDb.Posts.Where(x => x.PostId == post.PostId).FirstOrDefault();
                    if (DeletedPosts != null)
                    {
                        _postController.DeleteMultiplePosts(DeletedPosts.PostId);
                    }
                }
                _socialMediaDb.RemoveRange(DeletedPosts);
                _socialMediaDb.GroupPosts.RemoveRange(GroupPosts);
            }
            var DeletedGroup = new Deleted_Groups();
            DeletedGroup.CreatedById = Group.CreatedById;
            DeletedGroup.CreatedDate = Group.CreatedDate;
            DeletedGroup.Description = Group.Description;
            DeletedGroup.Name = Group.Name;
            DeletedGroup.GroupId = Group.GroupId;
            DeletedGroup.IsPublic = Group.IsPublic;
            _socialMediaDb.Deleted_Groups.Add(DeletedGroup);
            _socialMediaDb.Groups.Remove(Group);
            _socialMediaDb.SaveChanges();
            return Ok("Group Succesfully deleted!");
            
        }


    }
}
