using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Configurations;

public class FacilityConfiguration : IEntityTypeConfiguration<Facility>
{
    public void Configure(EntityTypeBuilder<Facility> builder)
    {
        builder.HasKey(e => e.Id);

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

        builder.HasOne(e => e.Category)
            .WithMany(c => c.Facilities)
            .HasForeignKey(e => e.FacilityCategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
