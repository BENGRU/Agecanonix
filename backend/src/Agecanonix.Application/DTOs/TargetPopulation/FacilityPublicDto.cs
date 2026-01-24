namespace Agecanonix.Application.DTOs.TargetPopulation;

public class TargetPopulationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
