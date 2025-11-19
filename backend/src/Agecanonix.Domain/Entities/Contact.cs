using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents an emergency contact for a resident
/// </summary>
public class Contact : BaseEntity
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? AlternativePhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string Relationship { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool CanPickUp { get; set; } = false;
    public string? Notes { get; set; }
    
    // Relations
    public Guid ResidentId { get; set; }
    public Resident Resident { get; set; } = null!;
}
