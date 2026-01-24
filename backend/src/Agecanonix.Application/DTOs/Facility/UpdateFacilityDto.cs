namespace Agecanonix.Application.DTOs.Facility;

public class UpdateFacilityDto
{
    public string Name { get; set; } = string.Empty;
    public string Siret { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public int Capacity { get; set; }
    public Guid ServiceTypeId { get; set; }
}
