namespace Agecanonix.Application.DTOs.Contact;

public class CreateContactDto
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? AlternativePhone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string Relationship { get; set; } = string.Empty;
    public int Priority { get; set; }
    public bool CanPickUp { get; set; }
    public string? Notes { get; set; }
    public Guid ResidentId { get; set; }
}
