using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Agecanonix.Infrastructure.Data;

/// <summary>
/// Generates sequential GUIDs to reduce index fragmentation (COMB-style).
/// </summary>
public class SequentialGuidValueGenerator : ValueGenerator<Guid>
{
    private static readonly object _lock = new();
    private static long _lastTimestamp;

    public override Guid Next(EntityEntry entry)
    {
        lock (_lock)
        {
            long timestamp = DateTime.UtcNow.Ticks;
            if (timestamp <= _lastTimestamp)
            {
                timestamp = _lastTimestamp + 1; // ensure monotonicity
            }

            _lastTimestamp = timestamp;

            byte[] guidArray = Guid.NewGuid().ToByteArray();
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            // Place timestamp in last 6 bytes to get ordering-friendly GUIDs
            Array.Reverse(timestampBytes);
            Array.Copy(timestampBytes, 0, guidArray, 10, 6);

            return new Guid(guidArray);
        }
    }

    public override bool GeneratesTemporaryValues => false;
}
