using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Configurations;

public class ContactConfiguration : IEntityTypeConfiguration<Contact>
{
    public void Configure(EntityTypeBuilder<Contact> builder)
    {
        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.LastName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.FirstName)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Phone)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(e => e.AlternativePhone)
            .HasMaxLength(20);
        
        builder.Property(e => e.Relationship)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(e => e.Priority)
            .IsRequired();
        
        builder.HasIndex(e => new { e.ResidentId, e.Priority });
        
        // Relationship configuration
        builder.HasOne(c => c.Resident)
            .WithMany(r => r.Contacts)
            .HasForeignKey(c => c.ResidentId)
            .OnDelete(DeleteBehavior.Cascade);
        
        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
