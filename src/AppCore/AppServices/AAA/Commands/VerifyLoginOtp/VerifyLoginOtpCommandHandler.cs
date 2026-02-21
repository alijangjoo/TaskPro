namespace Suspect.TaskPro.AppCore.AppServices.AAA.Commands.VerifyLoginOtp;

using OysterFx.AppCore.AppServices.Commands;
using OysterFx.AppCore.Shared.Commands.Common;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Abstractions;
using Suspect.TaskPro.AppCore.Domain.OtpCodesAggregate.Enums;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.VerifyLoginOtp;
using Suspect.TaskPro.AppCore.Shared.AAA.Services;
using Suspect.TaskPro.AppCore.Shared.AAA.Services.Dtos;
using Suspect.TaskPro.AppCore.Shared.OtpCodeContracts.OtpCodes.Commands;
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

        TokenResponse? tokenResponse = null;
        string userId = string.Empty;

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

            if (registeredUser is null || !registeredUser.IsSuccess)
            {
                verifyCommandResult.Verified = false;
                verifyCommandResult.Message = "خطا در ثبت نام کاربر";
                return await OkAsync(verifyCommandResult);
            }

            userId = registeredUser.UserId;
            tokenResponse = await _jwtTokenGeneratorService.AccessTokenAsync(command.MobileNumber, userId);
        }
        else
        {
            userId = userResponse.UserId!;
            tokenResponse = await _jwtTokenGeneratorService.AccessTokenAsync(command.MobileNumber, userId);
        }

        if (tokenResponse == null)
        {
            verifyCommandResult.Verified = false;
            verifyCommandResult.Message = "خطا در تولید توکن";
            return await OkAsync(verifyCommandResult);
        }

        // به‌روزرسانی نتیجه با TokenResponse کامل
        verifyCommandResult.Token = tokenResponse.AccessToken;
        verifyCommandResult.RefreshToken = tokenResponse.RefreshToken;
        verifyCommandResult.ExpiresIn = tokenResponse.ExpiresIn;
        verifyCommandResult.UserId = userId; // اضافه کردن UserId به نتیجه
        verifyCommandResult.Verified = true;
        verifyCommandResult.Message = "لاگین با موفقیت انجام شد";

        return await OkAsync(verifyCommandResult);
    }
}