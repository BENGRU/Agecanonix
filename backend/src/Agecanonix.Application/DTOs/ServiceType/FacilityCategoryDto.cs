namespace Agecanonix.Application.DTOs.ServiceType;

public class ServiceTypeDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid TargetPopulationId { get; set; }
    public string TargetPopulationName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
