using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using Kanban.Dto;
using AutoMapper;
using Kanban.Data;

namespace Kanban.Controllers {
    
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _repo;

        public UserController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _config = config;
            _repo = repo;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(UserForLoginDto userForRegisterDto)
        {
            userForRegisterDto.login = userForRegisterDto.login.ToLower();

            if (await _repo.UserExists(userForRegisterDto.login))
                return BadRequest("Username already exists");

            var userToCreate = _mapper.Map<User>(userForRegisterDto);
    
            var createdUser = await _repo.Register(userToCreate, userForRegisterDto.senha);
           
            return Ok("Usuário registrado com sucesso!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
            var userFromRepo = await _repo.Login(userForLoginDto.login
                .ToLower(), userForLoginDto.senha);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.login)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("SecurityKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<User>(userFromRepo);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token)
            });
        }
    }
}