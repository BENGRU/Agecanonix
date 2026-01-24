namespace Agecanonix.Application.DTOs.FacilityCategory;

public class UpdateFacilityCategoryDto
{
    public string Name { get; set; } = string.Empty;
    public Guid FacilityPublicId { get; set; }
}
