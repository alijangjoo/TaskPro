namespace Suspect.TaskPro.AppCore.AppServices.AAA.Commands.Logout;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.Logout;
using System.Threading.Tasks;

public class LogoutCommandHandler : CommandHandler<LogoutCommand, bool>
{
    private readonly IRefreshTokenCommandRepository _refreshTokenCommandRepository;

    public LogoutCommandHandler(IRefreshTokenCommandRepository refreshTokenCommandRepository)
    {
        _refreshTokenCommandRepository = refreshTokenCommandRepository;
    }

    public override async Task<CommandResult<bool>> Handle(LogoutCommand command)
    {
        await _refreshTokenCommandRepository.RevokeAsync(command.RefreshToken);
        await _refreshTokenCommandRepository.CommitAsync();
        return await OkAsync(true);
    }
}