namespace Suspect.TaskPro.AppCore.Shared.AAA.Services;

using Suspect.TaskPro.AppCore.Shared.AAA.Services.Dtos;

public interface IJwtTokenGeneratorService
{
    Task<TokenResponse?> AccessTokenAsync(string? mobileNumber, string userId);
    Task<TokenResponse?> RefreshTokenAsync(string refreshToken, string userId, string? mobileNumber);
}