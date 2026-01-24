namespace Agecanonix.Application.DTOs.ServiceType;

public class CreateServiceTypeDto
{
    public string Name { get; set; } = string.Empty;
    public Guid TargetPopulationId { get; set; }
}
