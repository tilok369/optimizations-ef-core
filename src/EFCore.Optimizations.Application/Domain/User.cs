namespace EFCore.Optimizations.Application.Domain;

public class User
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int ProfileId { get; set; }
    
    public virtual Profile Profile { get; set; } = null!;
}