using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SocialMedia.DTOs;
using SocialMedia.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SocialMedia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly SocialMediaDb _socialMediaDb;
        private readonly IConfiguration _config;
        public LoginController(SocialMediaDb socialMediaDb, IConfiguration config)
        {
            _socialMediaDb = socialMediaDb;
            _config = config;

        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult LogIn([FromBody]LogInDTO payload)
        {
           var payloadHashedPassword = HashPassword(payload.Password); 
           var existingUser = _socialMediaDb.Users
                .Where(u => u.Email == payload.Email
                && u.HashPassword == payloadHashedPassword)
                .SingleOrDefault();
            if (existingUser == null)
            {
                return NotFound("Email or password incorrect");
            }
            var jwt = GenerateJSONWebToken(existingUser);
            return Ok(jwt);
        }

        [AllowAnonymous]
        private string HashPassword(string password) 
        {
            using(var sha256 = SHA256.Create())
            {
                var passwordBytes = Encoding.UTF8.GetBytes(password);
                var hashedPasswordBytes = sha256.ComputeHash(passwordBytes);
                var base64HashedPasswordBytes = Convert.ToBase64String(hashedPasswordBytes);
                return base64HashedPasswordBytes;
            }
        }
        [AllowAnonymous]
        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
        new Claim(JwtRegisteredClaimNames.Email, userInfo.Email),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim("UserId", userInfo.UserId.ToString())

    };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
