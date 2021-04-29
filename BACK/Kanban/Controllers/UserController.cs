using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using System;

namespace Kanban.Controllers
{   
    [Route ("/login")]
    [ApiController]
    public class UserController
    {
                [HttpPost]
        public async Task<ActionResult<User>> Post ([FromBody] User value) {
            return value;
        }
        
    }
}