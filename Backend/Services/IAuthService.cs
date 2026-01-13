    
using AuthenticationModule.DTO.Request;
using AuthenticationModule.DTO.Response;

namespace AuthenticationModule.Services
{
    public interface IAuthService
    {
        Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request);
        Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken);
    }
}
