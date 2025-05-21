using Microsoft.EntityFrameworkCore;
using DesktopProgram.Models;
using System.Windows.Data;

namespace DesktopProgram.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Building> Buildings { get; set; }
        public DbSet<Resource> Resources { get; set; }
        public DbSet<BuildingResource> BuildingResources { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=appdata.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuildingResource>()
                .HasOne(br => br.Building)
                .WithMany(b => b.BuildingResources)
                .HasForeignKey(br => br.BuildingId);

            modelBuilder.Entity<BuildingResource>()
                .HasOne(br => br.Resource)
                .WithMany()
                .HasForeignKey(br => br.ResourceId);

            modelBuilder.Entity<Resource>().HasData(
                new Resource { Id = 1, Name = "Дерево", Cost = 100 },
                new Resource { Id = 2, Name = "Камень", Cost = 200 }
            );

            modelBuilder.Entity<Building>().HasData(
                new Building { Id = 1, Name = "Ферма", Status = "In Progress", Progress = 45 }
            );

            modelBuilder.Entity<BuildingResource>().HasData(
                new BuildingResource { Id = 1, BuildingId = 1, ResourceId = 1, Quantity = 20 },
                new BuildingResource { Id = 2, BuildingId = 1, ResourceId = 2, Quantity = 10 }
            );
        }
    }
}