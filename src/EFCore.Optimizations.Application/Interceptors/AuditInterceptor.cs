using System.Text.Json;
using EFCore.Optimizations.Application.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace EFCore.Optimizations.Application.Interceptors;

public class AuditInterceptor: SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData, 
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var context = eventData.Context;
        if (context == null) return await base.SavingChangesAsync(eventData, result, cancellationToken);

        var auditEntries = new List<AuditLog>();

        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                var audit = new AuditLog
                {
                    TableName = entry.Metadata?.GetTableName() ?? "No Table",
                    Operation = entry.State.ToString(),
                    PrimaryKey = GetPrimaryKeyValue(entry),
                    OldValues = entry.State == EntityState.Modified ? JsonSerializer.Serialize(GetOldValues(entry)) : null,
                    NewValues = entry.State == EntityState.Added ? JsonSerializer.Serialize(GetNewValues(entry)) : null,
                    User = "System" // Replace with actual user if available
                };

                auditEntries.Add(audit);
            }
        }

        if (auditEntries.Any())
        {
            context.Set<AuditLog>().AddRange(auditEntries);
        }
        
        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
    
    private int GetPrimaryKeyValue(EntityEntry entry)
    {
        return int.TryParse(entry.Properties
            .FirstOrDefault(p => 
                p.Metadata.IsPrimaryKey())?.CurrentValue?.ToString() ?? "0", out var result) ? result : 0;
    }

    private Dictionary<string, object> GetOldValues(EntityEntry entry)
    {
        return entry.Properties?
            .Where(p => p.IsModified)
            .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);
    }

    private Dictionary<string, object> GetNewValues(EntityEntry entry)
    {
        return entry.Properties
            .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
    }
}