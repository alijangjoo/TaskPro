namespace Suspect.TaskPro.Endpoints.Api.Auth;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OysterFx.Endpoints.Api.Controllers;
using Supect.TaskPro.Endpoints.Auth.Api.Models;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.GenerateRefreshToken;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.LoginByOtp;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.Logout;
using Suspect.TaskPro.AppCore.Shared.AAA.Commands.VerifyLoginOtp;
using Suspect.TaskPro.AppCore.Shared.AAA.Services.Dtos;

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

    [HttpPost("token/refresh")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var refreshTokenCommand = new GenerateRefreshTokenCommand()
        {
            MobileNumber = request.MobileNumber is null ? string.Empty : request.MobileNumber,
            RefreshToken = request.RefreshToken,
            UserId = request.UserId
        };

        var result = await SendCommand<GenerateRefreshTokenCommand, TokenResponse?>(refreshTokenCommand);

        return Ok(result);
    }
}