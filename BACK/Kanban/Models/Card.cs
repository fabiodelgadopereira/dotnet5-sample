using System;
using System.ComponentModel.DataAnnotations;

namespace Kanban.Models
{

    /*
    id: int | (guid [c#] | uuid [node])
    titulo : string, 
    conteudo: string, 
    lista: string*/
    public class Card
    {
        Guid id { get; set; }

        [StringLength(80, MinimumLength = 3, ErrorMessage = "Você deve especificar um titulo entre 3 e 80 caracteres")]
        public string titulo { get; set; }

        public string conteudo { get; set; }

        public string lista { get; set; }
    }
}