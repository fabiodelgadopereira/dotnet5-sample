using Microsoft.EntityFrameworkCore;
using Kanban.Models;

namespace Kanban.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=./SqliteDB.db");
        }
        
    }
}