using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Kanban.Models;

namespace Kanban.Data
{
    public class CardContext : DbContext
    {
        public DbSet<Card> Cards { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Data Source=DESKTOP-NE0O540;Initial Catalog=CadastroDB;Integrated Security=False;User ID=delgado;Password=...;");
        }
        
    }
}