namespace Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Commands;

public class RegisterUserResponse
{
    public string? UserId { get; set; }
    public bool IsSuccess { get; set; }
    public string? Message { get; set; }
}