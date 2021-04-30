using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Collections.Generic;
using Kanban.Data;
using Kanban.Dto;

namespace Kanban.Controllers {
    
    [Route ("/login")]
    [ApiController]
    public class UserController : ControllerBase {

        private readonly IConfiguration _config;
        private readonly AuthRepository _repository;

        public UserController (IConfiguration config , AuthRepository repository) {
            this._repository = repository;          
            this._config = config;
        }
        [HttpPost]
        public async Task<ActionResult<User>> Post ([FromBody] UserForLogin value) {

            var result = await _repository.Login (value.login, value.senha);

            var userToCreate = new User {
                login = value.login,
                senha = value.senha,
                Id = 1
            };
            if (result.login == userToCreate.login && result.senha == userToCreate.senha ) {
                return Ok (new {
                    token = GenerateJwtToken (userToCreate).Result
                });
            }

            return Unauthorized ();
          
        }

        private async Task<string> GenerateJwtToken (User user) {
            var claims = new List<Claim> {
                new Claim (ClaimTypes.NameIdentifier, user.Id.ToString ()),
                new Claim (ClaimTypes.Name, user.login)
            };

            var key = new SymmetricSecurityKey (Encoding.UTF8
                .GetBytes (_config.GetSection ("SecurityKey").Value));

            var creds = new SigningCredentials (key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity (claims),
                Expires = DateTime.Now.AddDays (1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler ();

            var token = tokenHandler.CreateToken (tokenDescriptor);

            return tokenHandler.WriteToken (token);
        }
    }
}
