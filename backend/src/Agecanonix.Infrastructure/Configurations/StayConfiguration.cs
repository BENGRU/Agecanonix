using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Configurations;

public class StayConfiguration : IEntityTypeConfiguration<Stay>
{
    public void Configure(EntityTypeBuilder<Stay> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.StartDate)
            .IsRequired();
        
        builder.Property(e => e.Type)
            .IsRequired();
        
        builder.HasIndex(e => new { e.ResidentId, e.StartDate });
        builder.HasIndex(e => new { e.FacilityId, e.StartDate });
        
        // Relationship configurations
        builder.HasOne(s => s.Resident)
            .WithMany(r => r.Stays)
            .HasForeignKey(s => s.ResidentId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(s => s.Facility)
            .WithMany(f => f.Stays)
            .HasForeignKey(s => s.FacilityId)
            .OnDelete(DeleteBehavior.Restrict);
        
        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
