namespace Suspect.TaskPro.AppCore.Domain.AAA.Entities;

using OysterFx.AppCore.Domain.Aggregates;

public sealed class RefreshToken : AggregateRoot
{
    public string UserId { get; private set; } = string.Empty;
    public string Token { get; private set; } = string.Empty;
    public DateTime ExpiresAt { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public bool IsRevoked { get; private set; }
    public string? MobileNumber { get; private set; }
    public string? ReplacedByToken { get; private set; }
    public DateTime? RevokedAt { get; private set; }

    private RefreshToken() { }
    private RefreshToken(
        string userId,
        string token,
        DateTime expiredAt,
        string mobileNumber,
        string replacedByToken)
    {
        UserId = userId;
        Token = token;
        ExpiresAt = expiredAt;
        CreatedAt = DateTime.UtcNow;
        IsRevoked = false;
        MobileNumber = mobileNumber;
        ReplacedByToken = replacedByToken;
        RevokedAt = null;
    }

    public static RefreshToken NewRefreshToken(
        string userId,
        string token,
        DateTime expiredAt,
        string mobileNumber,
        string replacedByToken) => new(userId, token, expiredAt, mobileNumber, replacedByToken);

    public void Revoke(string? replacedByToken = null)
    {
        IsRevoked = true;
        RevokedAt = DateTime.UtcNow;
        ReplacedByToken = replacedByToken;
    }

    public bool IsValid()
    {
        return !IsRevoked &&
               ExpiresAt > DateTime.UtcNow &&
               string.IsNullOrEmpty(ReplacedByToken);
    }

    public bool IsExpired()
    {
        return ExpiresAt <= DateTime.UtcNow;
    }
}