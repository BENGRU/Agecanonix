using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents a target population grouping service types (e.g., personnes âgées, personnes en situation de handicap).
/// </summary>
public class TargetPopulation : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public List<ServiceType> ServiceTypes { get; set; } = new();
}
