namespace Agecanonix.Application.DTOs.IndividualRelationship;

public class IndividualRelationshipDto
{
    public Guid Id { get; set; }
    public Guid SourceIndividualId { get; set; }
    public Guid RelatedIndividualId { get; set; }
    public string RelationshipType { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool CanPickUp { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
