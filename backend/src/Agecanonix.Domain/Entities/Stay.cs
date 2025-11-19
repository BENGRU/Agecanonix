using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents a stay of a resident in a facility
/// </summary>
public class Stay : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public StayType Type { get; set; }
    public string? Notes { get; set; }
    
    // Relations
    public Guid ResidentId { get; set; }
    public Resident Resident { get; set; } = null!;
    
    public Guid FacilityId { get; set; }
    public Facility Facility { get; set; } = null!;
}

/// <summary>
/// Types of stays in a nursing home
/// </summary>
public enum StayType
{
    ShortTermCare = 1,  // EHPAD Court séjour
    LongTermCare = 2    // EHPAD Long Séjour
}
