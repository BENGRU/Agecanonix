using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents an audience/public grouping facility categories (e.g., personnes âgées, personnes en situation de handicap).
/// </summary>
public class FacilityPublic : BaseEntity
{
    public string Name { get; set; } = string.Empty;

    public List<FacilityCategory> Categories { get; set; } = new();
}
