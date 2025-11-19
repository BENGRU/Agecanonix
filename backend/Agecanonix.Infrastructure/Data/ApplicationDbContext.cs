using Microsoft.EntityFrameworkCore;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Resident> Residents { get; set; }
    public DbSet<Contact> Contacts { get; set; }
    public DbSet<Stay> Stays { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuration Facility
        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Siret).IsRequired().HasMaxLength(14);
            entity.HasIndex(e => e.Siret).IsUnique();
            entity.Property(e => e.PostalCode).HasMaxLength(10);
            entity.Property(e => e.Phone).HasMaxLength(20);
        });

        // Configuration Resident
        modelBuilder.Entity<Resident>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.SocialSecurityNumber).IsRequired().HasMaxLength(15);
            entity.HasIndex(e => e.SocialSecurityNumber).IsUnique();
        });

        // Configuration Contact
        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Phone).IsRequired().HasMaxLength(20);
            entity.Property(e => e.AlternativePhone).HasMaxLength(20);
            entity.Property(e => e.Relationship).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Priority).IsRequired();
            entity.HasIndex(e => new { e.ResidentId, e.Priority });
            
            entity.HasOne(c => c.Resident)
                  .WithMany(r => r.Contacts)
                  .HasForeignKey(c => c.ResidentId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration Stay
        modelBuilder.Entity<Stay>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.StartDate).IsRequired();
            entity.Property(e => e.Type).IsRequired();
            entity.HasIndex(e => new { e.ResidentId, e.StartDate });
            entity.HasIndex(e => new { e.FacilityId, e.StartDate });
            
            entity.HasOne(s => s.Resident)
                  .WithMany(r => r.Stays)
                  .HasForeignKey(s => s.ResidentId)
                  .OnDelete(DeleteBehavior.Restrict);
                  
            entity.HasOne(s => s.Facility)
                  .WithMany(f => f.Stays)
                  .HasForeignKey(s => s.FacilityId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Global query filters for soft delete
        modelBuilder.Entity<Facility>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Resident>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Contact>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Stay>().HasQueryFilter(e => !e.IsDeleted);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Automatic timestamp updates
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Domain.Common.BaseEntity && 
                       (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (Domain.Common.BaseEntity)entityEntry.Entity;
            
            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
            }
            
            if (entityEntry.State == EntityState.Modified)
            {
                entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
