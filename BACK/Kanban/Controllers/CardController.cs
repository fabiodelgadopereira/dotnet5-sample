using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Kanban.Models;
using System;

namespace Kanban.Controllers
{   
    [Route ("/cards")]
    [ApiController]
    public class CardController
    {

        [HttpGet]
        public async Task<ActionResult<String>> Get () {
            return "True";
        }
        [HttpPost]
        public async Task<ActionResult<Card>> Post ([FromBody] Card value) {
            return value;
        }
        [HttpPut]
        public async Task<ActionResult<String>> Put (Guid id ) {
            return "true";
        }
        [HttpDelete]
        public async Task<ActionResult<String>> Delete (Guid id ) {
            return "True";
        }
    }
}