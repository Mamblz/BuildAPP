using Microsoft.EntityFrameworkCore;
using DesktopProgram.Models;
using System.Collections.Generic;
using System.Linq;

namespace DesktopProgram.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Building> Buildings { get; set; }
        public virtual DbSet<Resource> Resources { get; set; }
        public virtual DbSet<BuildingResource> BuildingResources { get; set; }
        public virtual DbSet<Project> Projects { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=appdata.db");
            }
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
                new Resource { Id = 1, Name = "�����o", Cost = 100 },
                new Resource { Id = 2, Name = "������", Cost = 200 }
            );

            modelBuilder.Entity<Building>().HasData(
                new Building { Id = 1, Name = "�����", Status = "In Progress", Progress = 45 }
            );

            modelBuilder.Entity<BuildingResource>().HasData(
                new BuildingResource { Id = 1, BuildingId = 1, ResourceId = 1, Quantity = 20 },
                new BuildingResource { Id = 2, BuildingId = 1, ResourceId = 2, Quantity = 10 }
            );
        }

        public static void SeedProjects()
        {
            using var context = new ApplicationDbContext();
            if (!context.Projects.Any())
            {
                var projects = new List<Project>
                {
                    new Project
                    {
                        Name = "����� �������� \"���������\"",
                        Status = "� ��������",
                        Budget = "12 ���",
                        ResourceUsage = "������: 500, ������: 200",
                        Timeline = "������ 2025 � ������� 2025"
                    },
                    new Project
                    {
                        Name = "������� ������ � ������",
                        Status = "�����������",
                        Budget = "25 ���",
                        ResourceUsage = "�����: 1000, �����: 500",
                        Timeline = "���� 2026 � ������ 2026"
                    }
                };

                context.Projects.AddRange(projects);
                context.SaveChanges();
            }
        }
    }
}
