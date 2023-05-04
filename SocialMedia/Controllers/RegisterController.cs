using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMedia.DTOs;
using SocialMedia.Models;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        public RegisterController(SocialMediaDb socialMediaDb)
        {
            _socialMediaDb = socialMediaDb;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register([FromBody]UserDTO User)
        {
            var user = new User();
            var UserHashedPassword = HashPassword(User.HashPassword);
            if (User != null )
            {
                if(CheckDuplicateEmail(User) == true)
                {
                    return BadRequest("Mail already exists");
                }
                user.Email = User.Email;
                user.FirstName = User.FirstName;
                user.LastName = User.LastName;
                user.Address = User.Address;
                user.City = User.City;
                user.ProfilePictureURL = User.ProfilePictureURL;
                user.CoverPictureURL = User.CoverPictureURL;
                user.Country = User.Country;
                user.BirthDate = User.BirthDate;
                user.PhoneNumber = User.PhoneNumber;
                user.JoinedDate = DateTime.Now;
                user.UserId = User.UserId;
                user.HashPassword = UserHashedPassword;
                _socialMediaDb.Add(user);
                _socialMediaDb.SaveChanges();
                return Ok("Registration Succesfull");
            }
            else
            {
                return BadRequest("User is missing information");
            }
        }
        [AllowAnonymous]
        private bool CheckDuplicateEmail(UserDTO User)
        {
            var userList = _socialMediaDb.Users.ToList();
            foreach(var user in userList)
            {
                if(user.Email == User.Email)
                {
                    return true;
                }
            }
            return false;
        }
        [AllowAnonymous]
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                var base64HashedPasswordBytes = Convert.ToBase64String(hashedPasswordBytes);
                return base64HashedPasswordBytes;
            }
        }


    }
}
