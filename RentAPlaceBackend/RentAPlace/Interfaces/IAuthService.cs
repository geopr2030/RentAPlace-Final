using RentAPlace.API.DTOs.Auth;

namespace RentAPlace.API.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
    }
}
