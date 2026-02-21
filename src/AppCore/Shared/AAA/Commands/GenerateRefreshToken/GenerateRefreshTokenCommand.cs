namespace Suspect.TaskPro.AppCore.Shared.AAA.Commands.GenerateRefreshToken;

using OysterFx.AppCore.Shared.Commands;
using Suspect.TaskPro.AppCore.Shared.AAA.Services.Dtos;

public class GenerateRefreshTokenCommand : ICommand<TokenResponse?>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
}