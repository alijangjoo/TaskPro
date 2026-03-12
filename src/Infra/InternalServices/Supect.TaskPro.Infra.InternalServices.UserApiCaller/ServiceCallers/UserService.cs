namespace Supect.TaskPro.Infra.InternalServices.UserApiCaller.ServiceCallers;

using Microsoft.Extensions.Options;
using OysterFx.Infra.Auth.JwtServices;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Abstractions;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Commands;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Constants;
using Supect.TaskPro.Infra.InternalServices.UserApiCaller.Models.Queries.GetUserIdByMobileNumber;

public class UserService : IUserService
{
    private readonly IOptions<UserServiceOptions> _userOptions;
    private readonly HttpService _httpService;
    private readonly IGenerateDafaultJwtTokenService _generateDafaultJwtTokenService;
    private readonly string _baseURL = string.Empty;

    public UserService(
        IOptions<UserServiceOptions> userOptions,
        HttpService httpService,
        IGenerateDafaultJwtTokenService generateDafaultJwtTokenService)
    {
        _userOptions = userOptions;
        _httpService = httpService;
        _generateDafaultJwtTokenService = generateDafaultJwtTokenService;
        _baseURL = _userOptions.Value.BaseURL;
    }

    public async Task<GetUserIdByMobileNumberResponse> GetUserIdByMobileNumber(
        GetUserIdByMobileNumberReqeust request,
        string baseUrl = "")
    {
        try
        {
            var url = string.IsNullOrEmpty(baseUrl) ? $"{_baseURL}{request.Path}" : $"{baseUrl}{request.Path}";
            var (success, error) = await _httpService.GetAsync<GetUserIdByMobileNumberResponse, List<string>>(url);

            if (error != null && error.Any())
            {
                // لاگ خطا
                return new GetUserIdByMobileNumberResponse
                {
                    IsSuccess = false,
                    Message = string.Join(", ", error)
                };
            }

            return success ?? new GetUserIdByMobileNumberResponse { IsSuccess = false };
        }
        catch (Exception ex)
        {
            // لاگ exception
            return new GetUserIdByMobileNumberResponse
            {
                IsSuccess = false,
                Message = $"خطا در ارتباط با سرویس کاربر: {ex.Message}"
            };
        }
    }

    public async Task<RegisterUserResponse> Register(RegisterUserRequest request, string baseUrl = "")
    {
        try
        {
            var url = string.IsNullOrEmpty(baseUrl) ? $"{_baseURL}api/Auth/register" : $"{baseUrl}api/Auth/register";
            var (success, error) = await _httpService.PostAsync<RegisterUserRequest, RegisterUserResponse, List<string>>(url, request);

            if (error != null && error.Any())
            {
                return new RegisterUserResponse
                {
                    IsSuccess = false,
                    Message = string.Join(", ", error),
                    UserId = string.Empty
                };
            }

            return success ?? new RegisterUserResponse
            {
                IsSuccess = false,
                Message = "پاسخی از سرویس دریافت نشد"
            };
        }
        catch (Exception ex)
        {
            return new RegisterUserResponse
            {
                IsSuccess = false,
                Message = $"خطا در ارتباط با سرویس کاربر: {ex.Message}",
                UserId = string.Empty
            };
        }
    }

    public async Task<GetUserByIdResponse> GetUserById(GetUserByIdRequest request, string baseUrl = "")
    {
        try
        {
            var url = string.IsNullOrEmpty(baseUrl)
                ? $"{_baseURL}api/Auth/user/{request.UserId}"
                : $"{baseUrl}api/Auth/user/{request.UserId}";

            var (success, error) = await _httpService.GetAsync<GetUserByIdResponse, List<string>>(url);

            if (error != null && error.Any())
            {
                return new GetUserByIdResponse
                {
                    IsSuccess = false,
                    Message = string.Join(", ", error)
                };
            }

            return success ?? new GetUserByIdResponse { IsSuccess = false };
        }
        catch (Exception ex)
        {
            return new GetUserByIdResponse
            {
                IsSuccess = false,
                Message = $"خطا در ارتباط با سرویس کاربر: {ex.Message}"
            };
        }
    }

    private string GetJwtToken() => _generateDafaultJwtTokenService.ExecuteAsync().Result;
}