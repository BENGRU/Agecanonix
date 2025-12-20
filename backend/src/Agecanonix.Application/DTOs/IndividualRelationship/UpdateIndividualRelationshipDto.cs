namespace Agecanonix.Application.DTOs.IndividualRelationship;

public class UpdateIndividualRelationshipDto
{
    public string RelationshipType { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool CanPickUp { get; set; }
    public string? Notes { get; set; }
}
