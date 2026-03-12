namespace Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Commands;

public class GetUserByIdResponse
{
    public string UserId { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}