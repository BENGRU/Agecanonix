namespace Agecanonix.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public bool IsDeleted { get; set; }
    
    /// <summary>
    /// Row version for optimistic concurrency control.
    /// Configured in Entity Framework Core as a timestamp column (managed by the database).
    /// </summary>
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();
}
