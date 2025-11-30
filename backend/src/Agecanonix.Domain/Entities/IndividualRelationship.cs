using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents a relationship between two individuals (e.g., parent-child, spouse, emergency contact)
/// </summary>
public class IndividualRelationship : BaseEntity
{
    // Individual who "has" the relationship (e.g., the resident)
    public Guid SourceIndividualId { get; set; }
    public Individual SourceIndividual { get; set; } = null!;
    
    // Individual who is the contact/related person
    public Guid RelatedIndividualId { get; set; }
    public Individual RelatedIndividual { get; set; } = null!;
    
    // Type of relationship from source's perspective (e.g., "Parent", "Spouse", "Friend")
    public string RelationshipType { get; set; } = string.Empty;
    
    // Priority for emergency contact (1 = highest priority)
    public int Priority { get; set; }
    
    // Whether this person can pick up the individual
    public bool CanPickUp { get; set; } = false;
    
    // Additional notes about the relationship
    public string? Notes { get; set; }
}
