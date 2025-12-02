using EcoAlert.DTOs;

namespace EcoAlert.Services.Interface
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterDto dto);
        Task<AuthResponseDto> LoginAsync(LoginDto dto);
        Task<UserDto> GetUserProfileAsync(int userId);
    }
}
