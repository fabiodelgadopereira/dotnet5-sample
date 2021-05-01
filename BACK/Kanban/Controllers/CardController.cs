using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kanban.Data;
using Kanban.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Kanban.Controllers {
    [Route ("/cards")]
    [ApiController]
    // [Authorize ()]
    public class CardController {

        [HttpGet]
        public async Task<List<Card>> Get () {

            using (var context = new CardContext ()) {
                return context.Cards.ToList ();
            }
        }

        [HttpPost]
        public async Task<ActionResult<Card>> Post ([FromBody] Card value) {

            using (var db = new CardContext ()) {
                db.Cards.Add (value);
                db.SaveChanges ();
            }
            return value;
        }

        [HttpPut]
        public async Task<ActionResult<Card>> Put (Guid id, [FromBody] Card value) {

            using (var db = new CardContext ()) {

                var element = db.Cards.First (x => x.id == id);

                // se o elemento não existir
                if (element != null) {
                    element.conteudo = value.conteudo;
                    element.titulo = value.titulo;
                    element.lista = value.lista;

                    db.Cards.Update (element);
                    db.SaveChanges ();

                    return element;
                }

                return element;

            }
        }

        [HttpDelete]
        public async Task<ActionResult<Card>> Delete (Guid id) {
            using (var db = new CardContext ()) {

                var element = db.Cards.First (x => x.id == id);

                // se o elemento não existir
                if (element != null) {
                    db.Cards.Remove (element);
                    db.SaveChanges ();
                }
                return element;
            }
        }
    }
}