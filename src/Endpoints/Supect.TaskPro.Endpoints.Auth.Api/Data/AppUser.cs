namespace Supect.TaskPro.Endpoints.Auth.Api.Data;

using Microsoft.AspNetCore.Identity;

public class AppUser : IdentityUser
{
    public string MobileNumber { get; set; } = default!;
}