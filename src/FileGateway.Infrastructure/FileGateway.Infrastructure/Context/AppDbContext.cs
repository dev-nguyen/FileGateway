using FileGateway.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FileGateway.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().HasKey(p => p.Id);
        modelBuilder.Entity<User>().HasIndex(p => p.Email);
    }
}
