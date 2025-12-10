namespace Suspect.TaskPro.AppCore.AppServices.AAA.VerifyLoginOtp;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Abstractions;
using Supect.TaskPro.Infra.PanelPayamak.Abstractions;
using Suspect.TaskPro.AppCore.Domain.OtpCodesAggregate.Enums;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.VerifyLoginOtp;
using Suspect.TaskPro.AppCore.Shared.AAA.Services;
using Suspect.TaskPro.AppCore.Shared.OtpCodeContracts.OtpCodes.Commands;
using System;
using System.Globalization;
using System.Threading.Tasks;

public class VerifyLoginOtpCommandHandler : CommandHandler<VerifyLoginOtpCommand, VerifyLoginOtpCommandResult?>
{
    private readonly IOtpCodeCommandRepository _otpCodeCommandRepository;
    private readonly IJwtTokenGeneratorService _jwtTokenGeneratorService;
    private readonly IUserService _userService;

    public VerifyLoginOtpCommandHandler(
        IOtpCodeCommandRepository otpCodeCommandRepository,
        IJwtTokenGeneratorService jwtTokenGeneratorService,
        IUserService userService)
    {
        _otpCodeCommandRepository = otpCodeCommandRepository;
        _jwtTokenGeneratorService = jwtTokenGeneratorService;
        _userService = userService;
    }

    public override async Task<CommandResult<VerifyLoginOtpCommandResult?>> Handle(VerifyLoginOtpCommand command)
    {
        VerifyLoginOtpCommandResult verifyCommandResult = new();
        var otpCode = await _otpCodeCommandRepository.GetByMobileNumberAndCode(command.MobileNumber!, command.Code!);

        if (otpCode is null || otpCode.Status != OtpStatus.Active)
        {
            verifyCommandResult.Verified = false;
            verifyCommandResult.Message = "کد وارد شده معتبر نیست";
            return await OkAsync(verifyCommandResult);
        }

        var verifyResult = otpCode.Verify(command.Code!);
        if (verifyResult is null || !verifyResult.IsSuccess)
        {
            verifyCommandResult.Verified = false;
            verifyCommandResult.Message = "کد وارد شده معتبر نیست";
            return await OkAsync(verifyCommandResult);
        }
        string jwtToken = string.Empty;

        var userResponse = await _userService.GetUserIdByMobileNumber(new()
        {
            MobileNumber = command.MobileNumber
        });


        if (userResponse is null || string.IsNullOrWhiteSpace(userResponse.UserId))
        {
            var registeredUser = await _userService.Register(new()
            {
                MobileNumber = command.MobileNumber
            });
            jwtToken = await _jwtTokenGeneratorService.ExecuteAsync(command.MobileNumber, registeredUser.Success.UserId);
        }
        else
        {
            jwtToken = await _jwtTokenGeneratorService.ExecuteAsync(command.MobileNumber, userResponse.UserId!);
        }

        verifyCommandResult.Token = jwtToken;
        verifyCommandResult.Verified = true;
        verifyCommandResult.Message = "لاگین با موفقیت انجام شد";

        return await OkAsync(verifyCommandResult);
    }
}