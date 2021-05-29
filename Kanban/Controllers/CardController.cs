using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanban.Data;
using Kanban.Helpers;
using Kanban.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Kanban.Controllers {

    [Route ("/cards")]
    [ApiController]
    [Authorize ()]
    public class CardController : ControllerBase {

        private readonly ILogger _logger;
        
        public CardController (ILogger<CardController> logger) {
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get () {
            try {
                await using (var context = new DataContext ()) {

                    return Ok (context.Cards.ToList ());
                }
            } catch (Exception x) {
                _logger.LogError (x.ToString ());
                return BadRequest ("Ops... Erro ao tentar retornar cards");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] Card value) {
            try {
                await using (var db = new DataContext ()) {
                    db.Cards.Add (value);
                    db.SaveChanges ();
                }
                return Ok (value);
            } catch (Exception x) {
                _logger.LogError (x.ToString ());
                return BadRequest ("Card não encontrado");
            }
        }

        [ServiceFilter (typeof (CustomActionFilter))]
        [HttpPut]
        public async Task<IActionResult> Put (Guid id, [FromBody] Card value) {

            await using (var db = new DataContext ()) {
                try {
                    var element = db.Cards.First (x => x.id == id);

                    // se o elemento não vazio
                    if (element != null) {
                        element.conteudo = value.conteudo;
                        element.titulo = value.titulo;
                        element.lista = value.lista;

                        db.Cards.Update (element);
                        db.SaveChanges ();

                        return Ok (element);
                    }
                    return BadRequest ("Card não encontrado");

                } catch (Exception x) {
                    _logger.LogError (x.ToString ());
                    return BadRequest ("Card não encontrado");
                }
            }
        }

        [ServiceFilter (typeof (CustomActionFilter))]
        [HttpDelete]
        public async Task<IActionResult> Delete (Guid id) {
            try {
                await using (var db = new DataContext ()) {

                    var element = db.Cards.First (x => x.id == id);

                    // se o elemento não é vazio
                    if (element != null) {
                        db.Cards.Remove (element);
                        db.SaveChanges ();
                        return Ok (element);
                    }

                    return BadRequest ();
                }
            } catch (Exception x) {
                _logger.LogError (x.ToString ());
                return BadRequest ("Card não encontrado");
            }
        }
    }
}