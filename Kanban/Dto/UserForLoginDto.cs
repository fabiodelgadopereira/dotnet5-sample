using System.ComponentModel.DataAnnotations;

namespace Kanban.Dto {
    public class UserForLoginDto {

        [Required]
        public string login { get; set; }

        [Required]
        public string senha { get; set; }
    }
}