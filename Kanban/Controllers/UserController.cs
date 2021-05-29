using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Kanban.Data;
using Kanban.Dto;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace Kanban.Controllers {

    [ApiController]
    public class UserController : ControllerBase {

        private readonly IConfiguration _config;
        private readonly IMapper _mapper;
        private readonly IAuthRepository _repo;
        private readonly ILogger _logger;

        public UserController (IAuthRepository repo, IConfiguration config, IMapper mapper, ILogger<CardController> logger) {
            _mapper = mapper;
            _config = config;
            _repo = repo;
            _logger = logger;
        }

        [HttpPost ("register")]
        public async Task<IActionResult> Register (UserForLoginDto userForRegisterDto) {
            try {
                userForRegisterDto.login = userForRegisterDto.login.ToLower ();

                if (await _repo.UserExists (userForRegisterDto.login))
                    return BadRequest ("Login já existe");

                var userToCreate = _mapper.Map<User> (userForRegisterDto);

                var createdUser = await _repo.Register (userToCreate, userForRegisterDto.senha);

                return Ok ("Usuário registrado com sucesso!");
                
            } catch (Exception x) {
                _logger.LogError (x.ToString ());
                return BadRequest ("Ops... Não foi possivel registrar o usuário");
            }
        }

        [HttpPost ("login")]
        public async Task<IActionResult> Login (UserForLoginDto userForLoginDto) {
            try {
                var userFromRepo = await _repo.Login (userForLoginDto.login
                    .ToLower (), userForLoginDto.senha);

                if (userFromRepo == null)
                    return Unauthorized ();

                var claims = new [] {
                    new Claim (ClaimTypes.NameIdentifier, userFromRepo.id.ToString ()),
                    new Claim (ClaimTypes.Name, userFromRepo.login)
                };

                var key = new SymmetricSecurityKey (Encoding.UTF8.GetBytes (_config.GetSection ("SecurityKey").Value));

                var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

                var tokenDescriptor = new SecurityTokenDescriptor {
                    Subject = new ClaimsIdentity (claims),
                    Expires = DateTime.Now.AddDays (1),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler ();

                var token = tokenHandler.CreateToken (tokenDescriptor);

                var user = _mapper.Map<User> (userFromRepo);

                return Ok (new {
                    token = tokenHandler.WriteToken (token)
                });

            } catch (Exception x) {
                _logger.LogError (x.ToString ());
                return BadRequest ("Ops... Não foi possivel fazer login");
            }
        }
    }
}