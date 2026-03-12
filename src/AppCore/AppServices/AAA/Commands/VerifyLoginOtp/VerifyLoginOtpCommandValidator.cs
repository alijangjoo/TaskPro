namespace Suspect.TaskPro.AppCore.AppServices.AAA.Commands.VerifyLoginOtp;

using FluentValidation;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.VerifyLoginOtp;

public class VerifyLoginOtpCommandValidator : AbstractValidator<VerifyLoginOtpCommand>
{
    public VerifyLoginOtpCommandValidator()
    {
    }
}