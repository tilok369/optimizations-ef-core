using EFCore.Optimizations.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EFCore.Optimizations.Application;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
    
    public virtual DbSet<Profile> Profiles { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>(entity => entity.HasKey(e => e.Id));
        modelBuilder.Entity<Profile>(entity => entity.Property(p=>p.Name).HasMaxLength(100));
        modelBuilder.Entity<Profile>(entity => entity.Property(p=>p.Email).HasMaxLength(100));
        modelBuilder.Entity<Profile>(entity => entity.Property(p=>p.Phone).HasMaxLength(20));
        modelBuilder.Entity<Profile>(entity => entity.Property(p=>p.Address).HasMaxLength(100));
        
        modelBuilder.Entity<Profile>(entity => entity.HasIndex(i => i.Email));
        
        
        modelBuilder.Entity<User>(entity => entity.HasKey(e => e.Id));
        modelBuilder.Entity<User>(entity => entity.Property(p=>p.UserName).HasMaxLength(20));
        modelBuilder.Entity<User>(entity => entity.Property(p=>p.Password).HasMaxLength(20));
        
        modelBuilder.Entity<AuditLog>(entity => entity.HasKey(e => e.Id));
        modelBuilder.Entity<AuditLog>(entity => entity.Property(p => p.Operation).HasMaxLength(10));
        modelBuilder.Entity<AuditLog>(entity => entity.Property(p => p.NewValues).HasMaxLength(1000));
        modelBuilder.Entity<AuditLog>(entity => entity.Property(p => p.OldValues).HasMaxLength(1000));
        modelBuilder.Entity<AuditLog>(entity => entity.Property(p => p.User).HasMaxLength(50));
        
        base.OnModelCreating(modelBuilder);
    }
}