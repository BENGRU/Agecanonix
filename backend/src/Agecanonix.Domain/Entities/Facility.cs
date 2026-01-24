using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents a nursing home facility (EHPAD)
/// </summary>
public class Facility : BaseEntity
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
    public ServiceType? ServiceType { get; set; }
}
