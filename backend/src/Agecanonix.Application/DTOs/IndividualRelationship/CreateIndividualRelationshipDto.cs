namespace Agecanonix.Application.DTOs.IndividualRelationship;

public class CreateIndividualRelationshipDto
{
    public Guid SourceIndividualId { get; set; }
    public Guid RelatedIndividualId { get; set; }
    public string RelationshipType { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool CanPickUp { get; set; }
    public string? Notes { get; set; }
}
