using Agecanonix.Domain.Common;

namespace Agecanonix.Domain.Entities;

/// <summary>
/// Represents a resident in a nursing home
/// </summary>
public class Resident : BaseEntity
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string SocialSecurityNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    
    // Relations
    public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
    public ICollection<Stay> Stays { get; set; } = new List<Stay>();
}
