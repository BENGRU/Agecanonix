using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents a category that can be assigned to a facility (e.g., EHPAD, MAS, FAM).
/// Categories are grouped under a public (audience).
/// </summary>
public class FacilityCategory : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid FacilityPublicId { get; set; }

    public FacilityPublic? Public { get; set; }
    public List<Facility> Facilities { get; set; } = new();
}
