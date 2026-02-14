using Microsoft.EntityFrameworkCore;
using Agecanonix.Application.Exceptions;

namespace Agecanonix.Application.Infrastructure;

/// <summary>
/// Extension methods for handling Entity Framework Core concurrency exceptions
/// </summary>
public static class ConcurrencyExceptionHandler
{
    /// <summary>
    /// Handles DbUpdateConcurrencyException by converting it to a ConcurrencyException
    /// </summary>
    public static void HandleConcurrencyException(DbUpdateConcurrencyException ex, string entityName, Guid entityId)
    {
        throw new ConcurrencyException(
            $"Unable to update {entityName} with ID {entityId}. The record was modified by another user. " +
            $"Please refresh and try again.",
            ex
        );
    }

    /// <summary>
    /// Wraps an async operation and handles concurrency exceptions
    /// </summary>
    public static async Task<T> ExecuteWithConcurrencyHandlingAsync<T>(
        Func<Task<T>> operation,
        string entityName,
        Guid entityId)
    {
        try
        {
            return await operation();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            HandleConcurrencyException(ex, entityName, entityId);
            throw; // Never reached due to throw in HandleConcurrencyException
        }
    }

    /// <summary>
    /// Wraps an async operation and handles concurrency exceptions
    /// </summary>
    public static async Task ExecuteWithConcurrencyHandlingAsync(
        Func<Task> operation,
        string entityName,
        Guid entityId)
    {
        try
        {
            await operation();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            HandleConcurrencyException(ex, entityName, entityId);
        }
    }
}
