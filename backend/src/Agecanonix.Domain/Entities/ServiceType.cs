using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents a service type that can be assigned to a facility (e.g., EHPAD, MAS, FAM).
/// Service types are grouped under a target population.
/// </summary>
public class ServiceType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid TargetPopulationId { get; set; }

    public TargetPopulation? TargetPopulation { get; set; }
    public List<Facility> Facilities { get; set; } = new();
}
