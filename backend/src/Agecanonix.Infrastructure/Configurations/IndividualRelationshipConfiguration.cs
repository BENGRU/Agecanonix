using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Agecanonix.Domain.Entities;

namespace Agecanonix.Infrastructure.Configurations;

public class IndividualRelationshipConfiguration : IEntityTypeConfiguration<IndividualRelationship>
{
    public void Configure(EntityTypeBuilder<IndividualRelationship> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.RelationshipType)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.Priority)
            .IsRequired();

        // Indexes for efficient queries
        builder.HasIndex(e => new { e.SourceIndividualId, e.Priority });
        builder.HasIndex(e => e.RelatedIndividualId);

        // Relationship configuration - source individual
        builder.HasOne(r => r.SourceIndividual)
            .WithMany(i => i.RelatedIndividuals)
            .HasForeignKey(r => r.SourceIndividualId)
            .OnDelete(DeleteBehavior.Restrict);

        // Relationship configuration - related individual
        builder.HasOne(r => r.RelatedIndividual)
            .WithMany(i => i.RelatedBy)
            .HasForeignKey(r => r.RelatedIndividualId)
            .OnDelete(DeleteBehavior.Restrict);

        // Global query filter for soft delete
        builder.HasQueryFilter(e => !e.IsDeleted);
    }
}
