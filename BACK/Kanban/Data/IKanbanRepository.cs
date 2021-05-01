  
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Kanban.Data
{
     public interface IKanbanRepository
    {
        void Add<T>(T entity) where T : class;
        void Delete<T>(T entity) where T : class;
        Task<bool> SaveAll();
        
    }
}