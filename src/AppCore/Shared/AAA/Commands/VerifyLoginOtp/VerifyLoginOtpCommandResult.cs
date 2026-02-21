namespace Suspect.TaskPro.AppCore.Shared.AAA.Commands.VerifyLoginOtp;

public class VerifyLoginOtpCommandResult
{
    public bool Verified { get; set; }
    public string? Token { get; set; }
    public string? Message { get; set; }
    public string? RefreshToken { get; set; }
    public int ExpiresIn { get; set; }
    public string? UserId { get; set; }
}