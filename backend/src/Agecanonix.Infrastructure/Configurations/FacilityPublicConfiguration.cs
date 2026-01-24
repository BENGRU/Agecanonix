using Agecanonix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agecanonix.Infrastructure.Configurations;

public class FacilityPublicConfiguration : IEntityTypeConfiguration<FacilityPublic>
{
    private static readonly DateTime SeedTimestamp = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    public static readonly Guid ElderlyPublicId = new("11111111-1111-1111-1111-111111111111");
    public static readonly Guid DisabilityPublicId = new("22222222-2222-2222-2222-222222222222");

    public void Configure(EntityTypeBuilder<FacilityPublic> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(e => e.Name).IsUnique();

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasData(
            new FacilityPublic
            {
                Id = ElderlyPublicId,
                Name = "Personnes âgées",
                CreatedAt = SeedTimestamp,
                CreatedBy = "seed"
            },
            new FacilityPublic
            {
                Id = DisabilityPublicId,
                Name = "Personnes en situation de handicap",
                CreatedAt = SeedTimestamp,
                CreatedBy = "seed"
            }
        );
    }
}
