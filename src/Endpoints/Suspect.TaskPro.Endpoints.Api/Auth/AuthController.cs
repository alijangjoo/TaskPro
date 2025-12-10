namespace Suspect.TaskPro.Endpoints.Api.Auth;

using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.VerifyLoginOtp;

[Route("api/auth")]
[ApiController]
public class AuthController : OysterFxController
{
    [HttpPost("login/otp")]
    public async Task<IActionResult> LoginByOtp([FromBody] LoginByOtpCommand request)
           => await SendCommand<LoginByOtpCommand, LoginByOtpCommandResult?>(
               LoginByOtpCommand.CreateInstance(request.MobileNumber));

    [HttpPost("login/otp/{mobileNumber}/verify")]
    public async Task<IActionResult> VerifyOtpLogin(
        [FromRoute] string mobileNumber,
        [FromBody] string? code)
        => await SendCommand<VerifyLoginOtpCommand, VerifyLoginOtpCommandResult?>(VerifyLoginOtpCommand.CreateInstance(mobileNumber, code));

    //[HttpDelete("logout")]
    //public async Task<IActionResult> Logout()
    //    => await SendCommand<LogoutCommand, LogoutCommandResult?>(new LogoutCommand());

    //[HttpGet("profile")]
    //[Authorize]
    //public async Task<IActionResult> GetProfile()
    //    => await SendQuery<GetUserProfileQuery, GetUserProfileQueryResult?>(new GetUserProfileQuery());
}