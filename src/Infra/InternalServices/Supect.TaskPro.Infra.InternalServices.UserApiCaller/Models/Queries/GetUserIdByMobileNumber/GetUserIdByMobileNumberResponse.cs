namespace Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Commands;

public class GetUserIdByMobileNumberResponse
{
    public string UserId { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}