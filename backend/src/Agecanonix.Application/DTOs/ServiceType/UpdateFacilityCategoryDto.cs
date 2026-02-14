namespace Agecanonix.Application.DTOs.ServiceType;

public class UpdateServiceTypeDto
{
    public string Name { get; set; } = string.Empty;
    public Guid TargetPopulationId { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
