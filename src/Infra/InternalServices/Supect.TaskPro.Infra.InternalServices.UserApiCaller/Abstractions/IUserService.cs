namespace Supect.TaskPro.Infra.InternalServices.UserApiCaller.Abstractions;

using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Commands;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Queries.GetUserIdByMobileNumber;

public interface IUserService
{
    Task<RegisterUserResponse> Register(RegisterUserRequest request, string baseUrl = "");
    Task<GetUserIdByMobileNumberResponse> GetUserIdByMobileNumber(GetUserIdByMobileNumberReqeust request, string baseUrl = "");
    Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, string baseUrl = "");
}