using Agecanonix.Domain.Entities;

namespace Agecanonix.Application.DTOs.Stay;

public class StayDto
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public StayType Type { get; set; }
    public string TypeDisplay => Type == StayType.ShortTermCare ? "Court séjour" : "Long séjour";
    public string? Notes { get; set; }
    public Guid ResidentId { get; set; }
    public Guid FacilityId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
