namespace Suspect.TaskPro.AppCore.Shared.AAA.Services.Dtos;

public class TokenInfo
{
    public string AccessToken { get; set; } = default!;
    public string? RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } // seconds
}