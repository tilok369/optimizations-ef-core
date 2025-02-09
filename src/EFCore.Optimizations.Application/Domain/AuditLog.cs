namespace EFCore.Optimizations.Application.Domain;

public class AuditLog
{
    public int Id { get; set; }
    public string TableName { get; set; } = string.Empty;
    public string Operation { get; set; } = string.Empty; // Insert, Update, Delete
    public int PrimaryKey { get; set; }  = 0;
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? User { get; set; }
}