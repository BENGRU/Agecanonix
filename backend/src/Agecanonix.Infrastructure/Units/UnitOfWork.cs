using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Entities;
using Agecanonix.Infrastructure.Data;
using Agecanonix.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Agecanonix.Infrastructure.Units;

/// <summary>
/// Unit of Work implementation for coordinating changes across multiple repositories
/// without automatic SaveChanges calls
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    // Unit of Work repositories that don't call SaveChanges automatically
    private IRepository<Individual>? _individualsRepository;
    private IRepository<IndividualRelationship>? _relationshipsRepository;
    private IRepository<Facility>? _facilitiesRepository;
    private IRepository<ServiceType>? _serviceTypesRepository;
    private IRepository<TargetPopulation>? _targetPopulationsRepository;

    // Transaction management
    private IDbContextTransaction? _transaction;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    // Repository properties with lazy initialization
    // These repositories don't call SaveChanges automatically
    public IRepository<Individual> Individuals =>
        _individualsRepository ??= new UnitOfWorkRepository<Individual>(_context);

    public IRepository<IndividualRelationship> Relationships =>
        _relationshipsRepository ??= new UnitOfWorkRepository<IndividualRelationship>(_context);

    public IRepository<Facility> Facilities =>
        _facilitiesRepository ??= new UnitOfWorkRepository<Facility>(_context);

    public IRepository<ServiceType> ServiceTypes =>
        _serviceTypesRepository ??= new UnitOfWorkRepository<ServiceType>(_context);

    public IRepository<TargetPopulation> TargetPopulations =>
        _targetPopulationsRepository ??= new UnitOfWorkRepository<TargetPopulation>(_context);

    /// <summary>
    /// Saves all changes made to all repositories in a single database transaction
    /// </summary>
    public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    /// <summary>
    /// Begins a new database transaction
    /// </summary>
    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    /// <summary>
    /// Commits the current transaction
    /// </summary>
    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await CompleteAsync(cancellationToken);
            if (_transaction != null)
            {
                await _transaction.CommitAsync(cancellationToken);
            }
        }
        catch
        {
            await RollbackTransactionAsync(cancellationToken);
            throw;
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    /// <summary>
    /// Rolls back the current transaction
    /// </summary>
    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(cancellationToken);
            }
        }
        finally
        {
            _transaction?.Dispose();
            _transaction = null;
        }
    }

    /// <summary>
    /// Disposes of the Unit of Work
    /// </summary>
    public void Dispose()
    {
        _transaction?.Dispose();
        _context?.Dispose();
    }

    /// <summary>
    /// Internal repository implementation for UnitOfWork
    /// that doesn't call SaveChanges automatically
    /// </summary>
    private class UnitOfWorkRepository<T> : IRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public UnitOfWorkRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> FindAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
            // NO SaveChanges - managed by IUnitOfWork.CompleteAsync()
            return entity;
        }

        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Update(entity);
            // NO SaveChanges - managed by IUnitOfWork.CompleteAsync()
            await Task.CompletedTask; // Make it awaitable
        }

        public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
        {
            _dbSet.Remove(entity);
            // NO SaveChanges - managed by IUnitOfWork.CompleteAsync()
            await Task.CompletedTask; // Make it awaitable
        }

        public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync([id], cancellationToken) != null;
        }

        public async Task<T?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => 
                EF.Property<Guid>(e, "Id") == id && 
                EF.Property<bool>(e, "IsDeleted") == true, 
                cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllDeletedAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.IgnoreQueryFilters()
                .Where(e => EF.Property<bool>(e, "IsDeleted") == true)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.IgnoreQueryFilters().ToListAsync(cancellationToken);
        }
    }
}
