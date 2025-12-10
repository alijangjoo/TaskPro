namespace Suspect.TaskPro.Endpoints.Api;

using OysterFx.Infra.Auth.UserServices;

public class FakeUserInfoService : IUserInfoService
{
    public string UserId => string.Empty;

    public string Username => string.Empty;

    public string? GetClaim(string claimType)
    {
        return string.Empty;
    }

    public string GetFirstName()
    {
        return string.Empty;
    }

    public string GetLastName()
    {
        return string.Empty;
    }

    public string GetUserAgent()
    {
        return string.Empty;
    }

    public string GetUserId()
    {
        return string.Empty;
    }

    public string GetUserIp()
    {
        return string.Empty;
    }

    public string GetUsername()
    {
        return string.Empty;
    }

    public bool IsCurrentUser(string userId)
    {
        return false;
    }

    public void LoadFromToken(string token)
    {
       
    }

    public string UserIdOrDefault()
    {
        return string.Empty;
    }

    public string UserIdOrDefault(string defaultValue)
    {
        return string.Empty;
    }
}