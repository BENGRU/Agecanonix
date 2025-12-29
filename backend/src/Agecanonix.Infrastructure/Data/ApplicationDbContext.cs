using System.Linq;
using Microsoft.EntityFrameworkCore;
using Agecanonix.Domain.Common;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Facility> Facilities { get; set; }
    public DbSet<Individual> Individuals { get; set; }
    public DbSet<IndividualRelationship> IndividualRelationships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all entity configurations from the assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Assign sequential GUID generator to all BaseEntity-derived types
        var baseEntityTypes = modelBuilder.Model.GetEntityTypes()
            .Where(t => typeof(BaseEntity).IsAssignableFrom(t.ClrType));

        foreach (var entityType in baseEntityTypes)
        {
            modelBuilder.Entity(entityType.ClrType)
                .Property(nameof(BaseEntity.Id))
                .ValueGeneratedOnAdd()
                .HasValueGenerator<SequentialGuidValueGenerator>();
        }
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
