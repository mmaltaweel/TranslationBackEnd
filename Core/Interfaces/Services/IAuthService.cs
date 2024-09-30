using API.Authentication;
using Core.DTO.RequestDTO;
using Core.DTO.ResponseDTO;

namespace Core.Interfaces;

public interface IAuthService
{
    Task<LoginResponse> LoginAsync(LoginModel model);
    Task<bool> RegisterTranslatorAsync(RegisterModel model);
    Task<bool> RegisterManagerAsync(RegisterModel model);
    Task<List<UserRequest>> FetchUsersAsync();
}