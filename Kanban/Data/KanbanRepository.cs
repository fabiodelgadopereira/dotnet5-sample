using System.Threading.Tasks;

namespace Kanban.Data
{
    public class KanbanRepository  : IKanbanRepository
    {
         private readonly DataContext _context;
        public KanbanRepository(DataContext context)
        {
            _context = context;
        }

        public void Add<T>(T entity) where T : class
        {
            _context.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _context.Remove(entity);
        }
          public async Task<bool> SaveAll()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        
    }
}