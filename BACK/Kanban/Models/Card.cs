using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kanban.Models
{

    /*
    id: int | (guid [c#] | uuid [node])
    titulo : string, 
    conteudo: string, 
    lista: string*/
    public class Card
    {
        [Key]
         public  Guid id { get; internal set; }

        [StringLength(80, MinimumLength = 3, ErrorMessage = "Você deve especificar um titulo entre 3 e 80 caracteres")]
        public string titulo { get; set; }

        [StringLength(400, MinimumLength = 3, ErrorMessage = "Você deve especificar um titulo entre 3 e 400 caracteres")]
        public string conteudo { get; set; }

        public string lista { get; set; }
    }
}