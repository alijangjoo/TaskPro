namespace Suspect.TaskPro.AppCore.AppServices.AAA.Commands.GenerateRefreshToken;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.GenerateRefreshToken;
using Suspect.TaskPro.AppCore.Shared.AAA.Services;
using Suspect.TaskPro.AppCore.Shared.AAA.Services.Dtos;
using System.Threading.Tasks;

public class GenerateRefreshTokenCommandHandler : CommandHandler<GenerateRefreshTokenCommand, TokenResponse?>
{
    private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;
    private readonly IRefreshTokenCommandRepository _refreshTokenCommandRepository;

    public GenerateRefreshTokenCommandHandler(
        IJwtTokenGeneratorService jwtTokenGeneratorService,
        IRefreshTokenCommandRepository refreshTokenCommandRepository)
    {
        _jwtTokenGeneratorService = jwtTokenGeneratorService;
        _refreshTokenCommandRepository = refreshTokenCommandRepository;
    }

    public override async Task<CommandResult<TokenResponse?>> Handle(GenerateRefreshTokenCommand command)
    {
        var refreshToken = await _refreshTokenCommandRepository.GetByTokenAsync(command.RefreshToken);

        if (refreshToken is null || !refreshToken.IsValid())
        {
            return await FailAsync("Refresh token is invalid or expired");
        }

        if (refreshToken.UserId != command.UserId)
        {
            return await FailAsync("Refresh token does not belong to this user");
        }

        var tokenResponse = await _jwtTokenGeneratorService.RefreshTokenAsync(
            command.RefreshToken,
            command.UserId,
            command.MobileNumber);

        if (tokenResponse == null)
        {
            return await FailAsync("Failed to generate new tokens");
        }

        return await OkAsync(tokenResponse);
    }
}
