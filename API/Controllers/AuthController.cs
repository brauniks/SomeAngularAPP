using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System;
using Microsoft.Extensions.Configuration;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            this._repo = repo;
            this._config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto UserForRegisterDto)
        {
             if(!String.IsNullOrEmpty(UserForRegisterDto.UserName))
                UserForRegisterDto.UserName = UserForRegisterDto.UserName.ToLower();
                 
            if(await _repo.UserExists(UserForRegisterDto.UserName))
            ModelState.AddModelError("Username", "Username already exists");

            if(!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var userToCreate = new User(){
                UserName = UserForRegisterDto.UserName
            };

            var createUser = await _repo.Register(userToCreate,UserForRegisterDto.Password);

            return StatusCode(201);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto UserForLoginDto)
        {
            var userFromRepo = await _repo.Login(UserForLoginDto.UserName.ToLower(), UserForLoginDto.Password);
            if(userFromRepo == null)
            {
                return Unauthorized();
            };
            
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value);
            var tokenDescriptor = new SecurityTokenDescriptor()
            { 
                Subject = new ClaimsIdentity( new Claim[] 
                {
                    new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()), 
                    new Claim(ClaimTypes.Name, userFromRepo.UserName)            
                }),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha512Signature)
            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new {tokenString});

        }
    }
}