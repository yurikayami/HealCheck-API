using Microsoft.EntityFrameworkCore;
using HealCheckAPI.Models;

namespace HealCheckAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Nutrient> Nutrients { get; set; }
        public DbSet<Analysis> Analysis { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships
            modelBuilder.Entity<Image>()
                .HasOne(i => i.User)
                .WithMany(u => u.Images)
                .HasForeignKey(i => i.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Analysis>()
                .HasOne(a => a.Image)
                .WithMany(i => i.Analyses)
                .HasForeignKey(a => a.ImageId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Analysis>()
                .HasOne(a => a.Nutrient)
                .WithMany(n => n.Analyses)
                .HasForeignKey(a => a.NutrientId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure unique constraints
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // Seed initial nutrients data
            modelBuilder.Entity<Nutrient>().HasData(
                new Nutrient { Id = 1, Name = "Calories", Unit = "kcal", Description = "Total energy" },
                new Nutrient { Id = 2, Name = "Protein", Unit = "gram", Description = "Protein" },
                new Nutrient { Id = 3, Name = "Fat", Unit = "gram", Description = "Fat" },
                new Nutrient { Id = 4, Name = "Carbohydrate", Unit = "gram", Description = "Carbohydrate" }
            );
        }
    }
}
