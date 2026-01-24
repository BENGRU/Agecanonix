using System;
using System.Threading;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Agecanonix.Infrastructure.Data;

/// <summary>
/// Generates sequential GUIDs to reduce index fragmentation (COMB-style).
/// </summary>
public class SequentialGuidValueGenerator : ValueGenerator<Guid>
{
    private static long _counter = DateTime.UtcNow.Ticks;

    public override Guid Next(EntityEntry entry)
    {
        long timestamp = Interlocked.Increment(ref _counter); // atomic monotonic counter

        byte[] guidArray = Guid.NewGuid().ToByteArray();
        byte[] timestampBytes = BitConverter.GetBytes(timestamp);

        // Place timestamp in first 6 bytes for true COMB ordering (SQL Server compares left-to-right)
        Array.Reverse(timestampBytes);
        Array.Copy(timestampBytes, 0, guidArray, 0, 6);

        return new Guid(guidArray);
    }

    public override bool GeneratesTemporaryValues => false;
}
