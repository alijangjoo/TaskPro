namespace Suspect.TaskPro.AppCore.AppServices.IntegrationTests.AAA.Commands.Common;

using OysterFx.Infra.Auth.UserServices;

public class FakeUserInfoService : IUserInfoService
{

    public string Username => "integration-test";

    public bool IsAuthenticated => true;

    public string UserId => "11111111-1111-1111-1111-111111111111";

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
        return UserId;
    }

    public string GetUserIp()
    {
        return string.Empty;
    }

    public string GetUsername()
    {
        return Username;
    }

    public bool IsCurrentUser(string userId)
    {
       return userId == UserId;
    }

    public void LoadFromToken(string token)
    { 
    }

    public string UserIdOrDefault()
    {
        return UserId;
    }

    public string UserIdOrDefault(string defaultValue)
    {
        return UserId ?? defaultValue;
    }
}