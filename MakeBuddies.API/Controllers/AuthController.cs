using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using MakeBuddies.API.Data;
using MakeBuddies.API.Dtos;
using MakeBuddies.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace MakeBuddies.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepo repo;
        private readonly IConfiguration config;
        public AuthController(IAuthRepo repo, IConfiguration config)
        {
            this.config = config;
            this.repo = repo;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserCreateDto userDto)
        {
            userDto.UserName = userDto.UserName.ToLower();
            if (await repo.UserExistAsync(userDto.UserName))
                return BadRequest("User Exists");

            var userToCreate = new User
            {
                Username = userDto.UserName
            };

            var createdUser = await repo.RegisterAsync(userToCreate, userDto.Password);

            return StatusCode(201);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var user = await repo.LoginAsync(userLoginDto.UserName.ToLower(), userLoginDto.Password);

            if(user == null)
            return Unauthorized();

            var claims = new Claim[]{
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)                
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("AppSettings:SecurityToken").Value));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = credentials
            };

            var tokeHandler = new JwtSecurityTokenHandler();

            var token = tokeHandler.CreateToken(tokenDescriptor);

            return Ok(new{
                token = tokeHandler.WriteToken(token)
            });


        }



    }
}