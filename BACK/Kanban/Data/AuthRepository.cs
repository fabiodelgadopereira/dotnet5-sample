using System.Threading.Tasks;
using Kanban.Models;

namespace Kanban.Data
{
    public class AuthRepository :   IAuthRepository
    {
       /* private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }*/
        public async Task<User> Login(string username, string password)
        {
           /* var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == username);

            if (user == null)
                return null;
            
            // if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            //     return null;

            return user;*/
            //  { "login" : "letscode", "senha":"lets@123" }
             return new User {
                login = "letscode",
                senha = "lets@123"
            };
        }
    
            public async Task<User> Register(User username, string password)
        {

             return new User {
                login = "letscode",
                senha = "lets@123"
            };
        }
        public async Task<bool> UserExists(string username)
        {
            return true;
        }
    }
}