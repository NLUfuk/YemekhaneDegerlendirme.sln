using Microsoft.EntityFrameworkCore;
using Yemekhane.Entities;

namespace Yemekhane.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // DbSet'ler
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Meal> Meals { get; set; } = null!;
        public DbSet<Rating> Ratings { get; set; } = null!;
        public DbSet<MealSuggestion> MealSuggestions { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meal>()     ///MEAL-RATING ILISKISI
                .HasMany(m => m.Ratings)
                .WithOne(r => r.Meal!)
                .HasForeignKey(r => r.MealId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Meal>()
                .Property(m => m.Id)
                .ValueGeneratedOnAdd(); // Identity

            // Rating tekrar oy engeli için opsiyonel unique index:
            modelBuilder.Entity<Rating>()
                .HasIndex(r => new { r.MealId, r.UserId })
                .IsUnique();
        }
    }
}
