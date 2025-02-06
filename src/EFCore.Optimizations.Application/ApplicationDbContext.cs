using EFCore.Optimizations.Application.Domain;
using Microsoft.EntityFrameworkCore;

namespace EFCore.Optimizations.Application;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Profile> Profiles { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(entity => entity.HasKey(e => e.Id));
        modelBuilder.Entity<User>(entity => entity.HasKey(e => e.Id));
        base.OnModelCreating(modelBuilder);
    }
}