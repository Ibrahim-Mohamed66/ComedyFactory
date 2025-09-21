
namespace Domain.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public bool IsRevoked { get; set; } = false;
    public bool IsActive => !IsRevoked && ExpiresAt > DateTime.UtcNow;
    public ApplicationUser? User { get; set; }
}
