using Agecanonix.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Agecanonix.Infrastructure.Configurations;

public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
{
    private static readonly DateTime SeedTimestamp = new(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);

    private static readonly Guid EhpadId = new("33333333-3333-3333-3333-333333333331");
    private static readonly Guid EhpaId = new("33333333-3333-3333-3333-333333333332");
    private static readonly Guid ResidencesAutonomieId = new("33333333-3333-3333-3333-333333333333");
    private static readonly Guid AccueilJourId = new("33333333-3333-3333-3333-333333333334");
    private static readonly Guid HebergementTemporaireId = new("33333333-3333-3333-3333-333333333335");
    private static readonly Guid SsiadId = new("33333333-3333-3333-3333-333333333336");
    private static readonly Guid EsaId = new("33333333-3333-3333-3333-333333333337");
    private static readonly Guid SpasadId = new("33333333-3333-3333-3333-333333333338");

    private static readonly Guid MasId = new("44444444-4444-4444-4444-444444444441");
    private static readonly Guid FamId = new("44444444-4444-4444-4444-444444444442");
    private static readonly Guid ImeId = new("44444444-4444-4444-4444-444444444443");
    private static readonly Guid IemId = new("44444444-4444-4444-4444-444444444444");
    private static readonly Guid ItepId = new("44444444-4444-4444-4444-444444444445");
    private static readonly Guid EeapId = new("44444444-4444-4444-4444-444444444446");
    private static readonly Guid SessadId = new("44444444-4444-4444-4444-444444444447");
    private static readonly Guid SavsId = new("44444444-4444-4444-4444-444444444448");
    private static readonly Guid SamsahId = new("44444444-4444-4444-4444-444444444449");
    private static readonly Guid FoyersVieId = new("44444444-4444-4444-4444-444444444450");
    private static readonly Guid FoyersHebergementId = new("44444444-4444-4444-4444-444444444451");
    private static readonly Guid EsatId = new("44444444-4444-4444-4444-444444444452");

    public void Configure(EntityTypeBuilder<ServiceType> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasOne(e => e.TargetPopulation)
            .WithMany(p => p.ServiceTypes)
            .HasForeignKey(e => e.TargetPopulationId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasQueryFilter(e => !e.IsDeleted);

        builder.HasData(GetSeedData());
    }

    private static IEnumerable<ServiceType> GetSeedData()
    {
        return new List<ServiceType>
        {
            new() { Id = EhpadId, Name = "EHPAD", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = EhpaId, Name = "EHPA", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = ResidencesAutonomieId, Name = "Résidences autonomie", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = AccueilJourId, Name = "Accueil de jour", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = HebergementTemporaireId, Name = "Hébergement temporaire", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = SsiadId, Name = "SSIAD", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = EsaId, Name = "ESA", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = SpasadId, Name = "SPASAD", TargetPopulationId = TargetPopulationConfiguration.ElderlyPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = MasId, Name = "MAS", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = FamId, Name = "FAM", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = ImeId, Name = "IME", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = IemId, Name = "IEM", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = ItepId, Name = "ITEP", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = EeapId, Name = "EEAP", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = SessadId, Name = "SESSAD", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = SavsId, Name = "SAVS", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = SamsahId, Name = "SAMSAH", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = FoyersVieId, Name = "Foyers de vie / foyers occupationnels", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = FoyersHebergementId, Name = "Foyers d'hébergement (travailleurs en ESAT)", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" },
            new() { Id = EsatId, Name = "ESAT", TargetPopulationId = TargetPopulationConfiguration.DisabilityPopulationId, CreatedAt = SeedTimestamp, CreatedBy = "seed" }
        };
    }
}
