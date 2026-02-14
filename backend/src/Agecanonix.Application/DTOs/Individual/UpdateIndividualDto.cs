namespace Agecanonix.Application.DTOs.Individual;

public class UpdateIndividualDto
{
    public string LastName { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string SocialSecurityNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
