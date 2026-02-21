namespace Suspect.TaskPro.Infra.Persistence.Postgres.Commands.RefreshTokens;

using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using OysterFx.Infra.Persistence.RDB.Commands;
using Suspect.TaskPro.AppCore.Domain.AAA.Entities;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

public class RefreshTokenCommandRepository : CommandRepository<RefreshToken, AppCommandDbContext>, IRefreshTokenCommandRepository
{
    public RefreshTokenCommandRepository(AppCommandDbContext dbContext) : base(dbContext)
    {
    }

    public async Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        await _dbContext.Set<RefreshToken>().AddAsync(refreshToken, cancellationToken);
    }

    public Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default)
    {
        var refreshToken = _dbContext.RefreshTokens.FirstOrDefault(x => x.Token == token);
        return Task.FromResult(refreshToken);
    }

    public Task<RefreshToken?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var refreshToken = _dbContext.RefreshTokens.FirstOrDefault(x => x.UserId == userId);
        return Task.FromResult(refreshToken);
    }

    public Task<IEnumerable<RefreshToken>> GetValidTokensByUserIdAsync(string userId, CancellationToken cancellationToken = default)
    {
        var refreshTokens = _dbContext.RefreshTokens.Where(x => x.UserId == userId && !x.IsRevoked && x.ExpiresAt > DateTime.UtcNow)?.ToList();
        return Task.FromResult(refreshTokens!.AsEnumerable());
    }

    public Task RemoveExpiredAsync(CancellationToken cancellationToken = default)
    {
        var refreshTokens = _dbContext.RefreshTokens.Where(x => x.ExpiresAt <= DateTime.UtcNow)?.ToList();
        _dbContext.RefreshTokens.RemoveRange(refreshTokens);
        return Task.FromResult(refreshTokens);
    }

    public async Task RevokeAllUserTokensAsync(string userId, CancellationToken cancellationToken = default)
    {
        var refreshTokens = await _dbContext.RefreshTokens.Where(x => x.UserId == userId && !x.IsRevoked)?.ToListAsync(cancellationToken)!;
        foreach (var token in refreshTokens)
            token.Revoke();
    }

    public async Task RevokeAsync(string token, string? replacedByToken = null, CancellationToken cancellationToken = default)
    {
        var refreshToken = await GetByTokenAsync(token, cancellationToken);
        if (refreshToken is not null)
        {
            refreshToken.Revoke(replacedByToken);
            await UpdateAsync(refreshToken, cancellationToken);
        }
    }

    public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        _dbContext.Set<RefreshToken>().Update(refreshToken);
        return Task.CompletedTask;
    }
}