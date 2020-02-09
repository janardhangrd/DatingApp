using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController:ControllerBase
    {
        private IAuthRepository _repo;
        private IConfiguration _config;

        public AuthController(IAuthRepository repository, IConfiguration configuration){
             _repo = repository;
            _config = configuration;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisteration userForRegisteration){
            //validation TBD

            userForRegisteration.UserName = userForRegisteration.UserName.ToLower();
            if(await _repo.UserExists(userForRegisteration.UserName)){
                return BadRequest("User Name already exists.");
            }

            var userToCreate = new User{
                UserName = userForRegisteration.UserName
            };

            var createdUser = await _repo.Register(userToCreate, userForRegisteration.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userForLoginDTO){

            //throw new Exception("Computer says No......");

            var userFromRepo = await _repo.Login(userForLoginDTO.UserName.ToLower(),userForLoginDTO.Password);

            if(userFromRepo == null){
                return Unauthorized();
            }
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = System.DateTime.Now.AddDays(2),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok( new {
                token = tokenHandler.WriteToken(token)
            }) ;
        }

    }
}