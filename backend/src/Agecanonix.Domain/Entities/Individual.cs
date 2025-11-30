using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents an individual (person) in the system
/// </summary>
public class Individual : BaseEntity
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string SocialSecurityNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    
    // Relations - relationships where this individual is the source
    public ICollection<IndividualRelationship> RelatedIndividuals { get; set; } = new List<IndividualRelationship>();
    
    // Relations - relationships where this individual is the related person
    public ICollection<IndividualRelationship> RelatedBy { get; set; } = new List<IndividualRelationship>();
}
