using System;
using Kanban.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace Kanban.services {
    class CustomActionFilter : ActionFilterAttribute {
        private readonly ILogger _logger;
        public CustomActionFilter (ILoggerFactory loggerFactory) {
            _logger = loggerFactory.CreateLogger ("CustomActionFilter");
        }
        public override void OnActionExecuting (ActionExecutingContext context) {
        //_logger.LogWarning(" - Inside OnActionExecuted method...");
            base.OnActionExecuting (context);
        }
        public override void OnActionExecuted (ActionExecutedContext context) {
            //_logger.LogWarning(" - Inside OnActionExecuted method...");
            base.OnActionExecuted (context);
        }
        public override void OnResultExecuting (ResultExecutingContext context) {
            //_logger.LogWarning(" - Inside OnActionExecuted method...");
            base.OnResultExecuting (context);
        }
        public override void OnResultExecuted (ResultExecutedContext context) {

            //> 01/01/2021 13:45:00 - Card 1 - Comprar Pão - Removido
            //<datetime> - Card <id> - <titulo> - <Remover|Alterar>
            // setup de variaveis
            string acao;
            string titulo;
            string id;

            // definindo da acao
            switch (context.ActionDescriptor.DisplayName.ToString ()) {
                case "Kanban.Controllers.CardController.Put (Kanban)":
                    acao = "Alterado";
                    break;
                case "Kanban.Controllers.CardController.Delete (Kanban)":
                    acao = "Removido";
                    break;
                default:
                    acao = "Vazio";
                    break;
            }

            //Buscando body response da chamada para preencher log
            Card c = (Card)Convert.ChangeType((context.Result as ObjectResult)?.Value, typeof(Card));

            // buscando o id do card
            id = c.id.ToString ();

            //  buscando o titulo do card
            titulo = c.titulo;

            _logger.LogWarning (DateTime.Now.ToString ("dd/MM/yyyy hh:mm:ss") + " - Card " + id + " - " + titulo + " - " + acao);

            base.OnResultExecuted (context);
        }

    }
}