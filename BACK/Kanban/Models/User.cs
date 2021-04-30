using Microsoft.AspNetCore.Identity;

namespace Kanban.Models
{
    public class User : IdentityUser<int>
    {
        public string login { get; set; }

        public string senha { get; set; }
    }
}