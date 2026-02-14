using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Agecanonix.Domain.Common;

namespace Agecanonix.Infrastructure.Configurations;

/// <summary>
/// Base configuration for all entities inheriting from BaseEntity.
/// Applies common property configurations across the domain model.
/// </summary>
public static class BaseEntityConfiguration
{
    /// <summary>
    /// Configures common properties for BaseEntity-derived entities.
    /// Call this in the OnModelCreating method of each entity configuration.
    /// </summary>
    public static void ConfigureBaseEntity<TEntity>(EntityTypeBuilder<TEntity> builder)
        where TEntity : BaseEntity
    {
        // Audit fields
        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.UpdatedAt)
            .IsRequired(false);

        builder.Property(e => e.CreatedBy)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.UpdatedBy)
            .IsRequired(false)
            .HasMaxLength(255);

        // Soft delete flag
        builder.Property(e => e.IsDeleted)
            .IsRequired();

        // Index on IsDeleted for query filter performance
        // Since every query applies HasQueryFilter(e => !e.IsDeleted),
        // an index on IsDeleted significantly improves query performance
        builder.HasIndex(e => e.IsDeleted);

        // Row version for optimistic concurrency control
        builder.Property(e => e.RowVersion)
            .IsRowVersion();
    }
}
