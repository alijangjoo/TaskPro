namespace Suspect.TaskPro.AppCore.AppServices.AAA.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Suspect.TaskPro.AppCore.Domain.AAA.Entities;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands;
using Suspect.TaskPro.AppCore.Shared.AAA.Services;
using Suspect.TaskPro.AppCore.Shared.AAA.Services.Dtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

public class JwtTokenGeneratorService : IJwtTokenGeneratorService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenCommandRepository _refreshTokenCommandRepository;
    public JwtTokenGeneratorService(IConfiguration configuration, IRefreshTokenCommandRepository refreshTokenCommandRepository)
    {
        _configuration = configuration;
        _refreshTokenCommandRepository = refreshTokenCommandRepository;
    }

    public async Task<TokenResponse?> AccessTokenAsync(string? mobileNumber, string userId)
    {
        var accessToken = GenerateAccessToken(mobileNumber, userId);
        var refreshToken = GenerateRefreshToken();

        await SaveRefreshTokenAsync(userId, refreshToken, mobileNumber!);

        return new TokenResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
            ExpiresIn = GetAccessTokenExpirationMinutes() * 60
        };
    }

    public async Task<TokenResponse?> RefreshTokenAsync(string refreshToken, string userId, string? mobileNumber)
    {
        var newRefreshToken = await UpdateRefreshTokenAsync(userId, refreshToken);

        var newAccessToken = GenerateAccessToken(mobileNumber, userId);

        return new TokenResponse
        {
            AccessToken = newAccessToken,
            RefreshToken = newRefreshToken,
            ExpiresIn = GetAccessTokenExpirationMinutes() * 60
        };
    }

    private string GenerateAccessToken(string? mobileNumber, string userId)
    {
        var claims = new List<Claim>
        {
            new Claim("MobileNumber", mobileNumber ?? string.Empty),
            new Claim("UserId", userId),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var jwtSecret = _configuration["Jwt:Secret"];
        var jwtIssuer = _configuration["Jwt:Issuer"];
        var jwtAudience = _configuration["Jwt:Audience"];
        var expirationMinutes = GetAccessTokenExpirationMinutes();

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.UtcNow.AddMinutes(expirationMinutes);

        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: expires,
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }

    private int GetAccessTokenExpirationMinutes()
    {
        int.TryParse(_configuration["Jwt:AccessTokenExpirationMinutes"], out int expirationMinutes);
        return expirationMinutes > 0 ? expirationMinutes : 15; // پیش‌فرض 15 دقیقه
    }

    private int GetRefreshTokenExpirationDays()
    {
        int.TryParse(_configuration["Jwt:RefreshTokenExpirationDays"], out int expirationDays);
        return expirationDays > 0 ? expirationDays : 7;
    }

    private async Task SaveRefreshTokenAsync(string userId, string token, string mobileNumber)
    {
        var expiredAt = DateTime.UtcNow.AddDays(GetRefreshTokenExpirationDays());

        RefreshToken refreshToken = RefreshToken.NewRefreshToken(
            userId,
            token,
            expiredAt,
            mobileNumber,
            string.Empty);
        await _refreshTokenCommandRepository.InsertAsync(refreshToken);
        await _refreshTokenCommandRepository.CommitAsync();

        await Task.CompletedTask;
    }

    private async Task<string> UpdateRefreshTokenAsync(string userId, string oldRefreshToken)
    {
        var newRefreshToken = GenerateRefreshToken();
        var expiredAt = DateTime.UtcNow.AddDays(GetRefreshTokenExpirationDays());

        var oldToken = await _refreshTokenCommandRepository.GetByTokenAsync(oldRefreshToken);
        if (oldToken is not null)
        {
            oldToken.Revoke(newRefreshToken);
            await _refreshTokenCommandRepository.UpdateAsync(oldToken);
        }

        RefreshToken newToken = RefreshToken.NewRefreshToken(
            userId,
            newRefreshToken,
            expiredAt,
            oldToken?.MobileNumber ?? string.Empty,
            string.Empty);

        await _refreshTokenCommandRepository.InsertAsync(newToken);
        await _refreshTokenCommandRepository.CommitAsync();
        return newRefreshToken;
    }
}