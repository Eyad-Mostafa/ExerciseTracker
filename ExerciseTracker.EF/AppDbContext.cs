using ExerciseTracker.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.EF;

public class AppDbContext : DbContext
{
    public DbSet<Exercise> Exercises { get; set; } = null!;
    public DbSet<AuthenticationRequest> AuthenticationRequests { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Exercise>().ToTable("Exercises");
        modelBuilder.Entity<Exercise>().Property(e => e.Comments).HasMaxLength(400);
        modelBuilder.Entity<AuthenticationRequest>().ToTable("Users");
    }
}
