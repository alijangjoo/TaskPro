namespace Supect.TaskPro.Endpoints.Auth.Api.Controllers;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Supect.TaskPro.Endpoints.Auth.Api.Data;
using Supect.TaskPro.Endpoints.Auth.Api.Models;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<AppUser> _userManager;

    public AuthController(UserManager<AppUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existing = await _userManager.Users
            .FirstOrDefaultAsync(u => u.MobileNumber == request.MobileNumber);

        if (existing != null)
            return BadRequest("Mobile number already exists.");

        var user = new AppUser
        {
            MobileNumber = request.MobileNumber,
            UserName = request.MobileNumber,
            PhoneNumber = request.MobileNumber,
            PhoneNumberConfirmed = true
        };

        var result = await _userManager.CreateAsync(user);

        if (!result.Succeeded)
            return BadRequest(result.Errors);

        return Ok(new UserIdResponse(user.Id));
    }

    [HttpGet("by-mobile/{mobileNumber}")]
    public async Task<IActionResult> GetUserIdByMobile(string mobileNumber)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.MobileNumber == mobileNumber);

        if (user == null)
            return NotFound("User not found.");

        return Ok(new UserIdResponse(user.Id));
    }
}