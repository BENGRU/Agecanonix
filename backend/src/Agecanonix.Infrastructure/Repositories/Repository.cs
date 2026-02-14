using System.Linq.Expressions;
using Agecanonix.Application.Interfaces;
using Agecanonix.Domain.Common;
using Agecanonix.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Agecanonix.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly ApplicationDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
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

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return entity;
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(T entity, CancellationToken cancellationToken = default)
    {
        // Check if entity inherits from BaseEntity to implement soft delete
        if (entity is BaseEntity baseEntity)
        {
            // Soft delete: mark IsDeleted = true and update audit fields
            baseEntity.IsDeleted = true;
            baseEntity.UpdatedAt = DateTime.UtcNow;
            baseEntity.UpdatedBy = "system";
            
            _dbSet.Update(entity);
        }
        else
        {
            // Hard delete for non-BaseEntity entities
            _dbSet.Remove(entity);
        }
        
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync([id], cancellationToken) != null;
    }

    /// <summary>
    /// Get a deleted entity by ID (ignores soft delete filter).
    /// Used for audit/history purposes.
    /// </summary>
    public async Task<T?> GetDeletedByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.IgnoreQueryFilters().FirstOrDefaultAsync(e => 
            EF.Property<Guid>(e, "Id") == id && 
            EF.Property<bool>(e, "IsDeleted") == true, 
            cancellationToken);
    }

    /// <summary>
    /// Get all deleted entities (ignores soft delete filter).
    /// Used for audit/history purposes.
    /// </summary>
    public async Task<IEnumerable<T>> GetAllDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.IgnoreQueryFilters()
            .Where(e => EF.Property<bool>(e, "IsDeleted") == true)
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// Get all entities including deleted ones (ignores soft delete filter).
    /// Result will include both active and deleted entities.
    /// </summary>
    public async Task<IEnumerable<T>> GetAllIncludingDeletedAsync(CancellationToken cancellationToken = default)
    {
        return await _dbSet.IgnoreQueryFilters().ToListAsync(cancellationToken);
    }
}
