using Microsoft.EntityFrameworkCore;
using DesktopProgram.Models;

namespace DesktopProgram.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=appdata.db");
        }
    }
}