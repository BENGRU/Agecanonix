namespace Agecanonix.Application.DTOs.IndividualRelationship;

public class UpdateIndividualRelationshipDto
{
    public string RelationshipType { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool IsEmergencyContact { get; set; }
    public string? Notes { get; set; }
}
