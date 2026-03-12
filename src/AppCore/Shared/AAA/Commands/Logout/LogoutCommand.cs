namespace Suspect.TaskPro.AppCore.Shared.AAA.Commands.Logout;

using OysterFx.AppCore.Shared.Commands;

public class LogoutCommand : ICommand<bool>
{
    public string RefreshToken { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
}