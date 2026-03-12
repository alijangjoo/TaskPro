namespace Supect.TaskPro.Endpoints.Auth.Api.Models;

public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
}