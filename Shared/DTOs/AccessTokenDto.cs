namespace Shared.DTOs;

public class AccessTokenDto
{
    public string AccessToken { get; set; } = null!;
    public string AccessType { get; set; } = null!;
    public DateTime ExpiresAt { get; set; }
}