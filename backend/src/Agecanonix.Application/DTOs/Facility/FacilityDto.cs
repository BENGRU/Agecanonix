namespace Agecanonix.Application.DTOs.Facility;

public class FacilityDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Siret { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int Capacity { get; set; }
    public Guid FacilityCategoryId { get; set; }
    public string FacilityCategoryName { get; set; } = string.Empty;
    public Guid FacilityPublicId { get; set; }
    public string FacilityPublicName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
