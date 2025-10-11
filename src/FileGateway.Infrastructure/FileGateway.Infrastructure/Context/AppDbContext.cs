using FileGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileGateway.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public DbSet<Secret> Secrets { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<User>().HasKey(p => p.Id);
        modelBuilder.Entity<User>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Secret>().HasKey(p => p.Id);
        modelBuilder.Entity<Secret>().HasIndex(p => p.FileName)
            .IsUnique();
        modelBuilder.Entity<Secret>().HasIndex(p => p.Token)
            .IsUnique();

        modelBuilder.Entity<Secret>().Property(p => p.Id)
            .ValueGeneratedOnAdd();

    }
}
