using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private IMapper _mapper;

        public AuthController(IAuthRepository repository, IConfiguration configuration, IMapper mapper){
             _repo = repository;
            _config = configuration;
            _mapper = mapper;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto){
            //validation TBD

            userForRegisterDto.UserName = userForRegisterDto.UserName.ToLower();
            if(await _repo.UserExists(userForRegisterDto.UserName)){
                return BadRequest("User Name already exists.");
            }

            var userToCreate = _mapper.Map<User>(userForRegisterDto);

            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);
            var userToReturn = _mapper.Map<UserForDetailsDto>(createdUser);
            return CreatedAtRoute("GetUser",new { controller = "Users", id = createdUser.Id}, userToReturn);
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

            var user = _mapper.Map<UserForListDto>(userFromRepo);

            return Ok( new {
                token = tokenHandler.WriteToken(token),
                user
            }) ;
        }

    }
}