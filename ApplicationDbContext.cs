using Microsoft.EntityFrameworkCore;
using BuildFlowApp.Models;

namespace BuildFlowApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=buildflow.db");
    }
}