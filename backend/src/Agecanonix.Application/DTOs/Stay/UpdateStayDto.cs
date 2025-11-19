using Agecanonix.Domain.Entities;

namespace Agecanonix.Application.DTOs.Stay;

public class UpdateStayDto
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public StayType Type { get; set; }
    public string? Notes { get; set; }
}
