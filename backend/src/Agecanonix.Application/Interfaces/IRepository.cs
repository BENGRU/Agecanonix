using System.Linq.Expressions;

namespace Agecanonix.Application.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    Task<T> AddAsync(T entity, CancellationToken cancellationToken = default);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task DeleteAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get a deleted entity by ID (ignores soft delete filter).
    /// Used for audit/history purposes.
    /// </summary>
    Task<T?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all deleted entities (ignores soft delete filter).
    /// Used for audit/history purposes.
    /// </summary>
    Task<IEnumerable<T>> GetAllDeletedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Get all entities including deleted ones (ignores soft delete filter).
    /// Result will include both active and deleted entities.
    /// </summary>
    Task<IEnumerable<T>> GetAllIncludingDeletedAsync(CancellationToken cancellationToken = default);
}
