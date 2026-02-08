using Agecanonix.Domain.Entities;

namespace Agecanonix.Application.Interfaces;

/// <summary>
/// Unit of Work pattern implementation for managing multiple repositories
/// and coordinating changes across different entities
/// </summary>
public interface IUnitOfWork : IDisposable
{
    // Repository properties for each entity type
    IRepository<Individual> Individuals { get; }
    IRepository<IndividualRelationship> Relationships { get; }
    IRepository<Facility> Facilities { get; }
    IRepository<ServiceType> ServiceTypes { get; }
    IRepository<TargetPopulation> TargetPopulations { get; }

    /// <summary>
    /// Saves all changes made to all repositories in a single database transaction
    /// </summary>
    Task<int> CompleteAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
