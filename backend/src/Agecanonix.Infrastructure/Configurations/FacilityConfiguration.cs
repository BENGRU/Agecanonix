using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Configurations;

public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.HasKey(e => e.Id);

        // Configure base entity properties (CreatedAt, UpdatedAt, CreatedBy, UpdatedBy, IsDeleted, RowVersion)
        BaseEntityConfiguration.ConfigureBaseEntity(builder);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.Siret)
            .IsRequired()
            .HasMaxLength(14);

        builder.HasIndex(e => e.Siret)
            .IsUnique();

        builder.Property(e => e.PostalCode)
            .HasMaxLength(10);

        builder.Property(e => e.Phone)
            .HasMaxLength(20);

        builder.HasOne(e => e.ServiceType)
            .WithMany(s => s.Facilities)
            .HasForeignKey(e => e.ServiceTypeId)
            .OnDelete(DeleteBehavior.Restrict);

        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
