namespace Suspect.TaskPro.AppCore.Shared.AAA.Commands;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Domain.AAA.Entities;

public interface IRefreshTokenCommandRepository : ICommandRepository<RefreshToken, long>
{
    Task<RefreshToken?> GetByTokenAsync(string token, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task<IEnumerable<RefreshToken>> GetValidTokensByUserIdAsync(string userId, CancellationToken cancellationToken = default);
    Task AddAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task RevokeAsync(string token, string? replacedByToken = null, CancellationToken cancellationToken = default);
    Task RevokeAllUserTokensAsync(string userId, CancellationToken cancellationToken = default);
    Task RemoveExpiredAsync(CancellationToken cancellationToken = default);
}