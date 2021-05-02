using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Kanban.Models;
using Microsoft.Data.Sqlite;

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