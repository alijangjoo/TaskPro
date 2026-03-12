namespace Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Queries.GetUserIdByMobileNumber;

public class GetUserIdByMobileNumberReqeust
{
    public string MobileNumber { get; set; } = string.Empty;
    public string Path => $"api/Auth/by-mobile/{MobileNumber}";
}
