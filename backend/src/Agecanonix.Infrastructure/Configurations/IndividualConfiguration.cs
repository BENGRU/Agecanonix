using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Configurations;

public class IndividualConfiguration : IEntityTypeConfiguration<Individual>
{
    public void Configure(EntityTypeBuilder<Individual> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.SocialSecurityNumber)
            .IsRequired()
            .HasMaxLength(15);
        
        builder.HasIndex(e => e.SocialSecurityNumber)
            .IsUnique();
        
        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
