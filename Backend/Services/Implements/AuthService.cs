using AuthenticationModule.DTO.Request;
using AuthenticationModule.DTO.Response;
using AuthenticationModule.Helper;
using AuthenticationModule.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationModule.Services.Implements
{
    public class AuthService : IAuthService
    {
        private readonly LoginPermissionContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHistory _history;

        public AuthService(LoginPermissionContext context, JwtHelper jwtHelper, IHttpContextAccessor httpContextAccessor,IHistory history)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _httpContextAccessor = httpContextAccessor;
            _history = history;
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO request)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username == request.Username);
            if (user == null)
            {
                await _history.SaveLoginHistory(null, false);
                throw new Exception("User không tồn tại");
            }
            if (!user.IsActive || user.IsLocked)
            {
                await _history.SaveLoginHistory(null, false);
                throw new Exception("User bị khoá");
            }
            if (!PasswordHelper.Verify(request.Password, user.PasswordHash))
            {
                await _history.SaveLoginHistory(user.Id, false);
                throw new Exception("Sai mật khẩu");
            }
            await _history.SaveLoginHistory(user.Id, true);
            var refreshTokenValue = Guid.NewGuid().ToString();
            var refreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                Token = refreshTokenValue,
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return new LoginResponseDTO
            {
                UserId = user.Id,
                AccessToken = _jwtHelper.GenerateToken(user.Id),
                RefreshToken = refreshTokenValue
            };
        }

        public async Task<RegisterResponseDTO> RegisterAsync(RegisterRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) ||
                string.IsNullOrWhiteSpace(request.Email) ||
                string.IsNullOrWhiteSpace(request.Password))
            {
                throw new Exception("Dữ liệu không hợp lệ");
            } 
            var isExist = await _context.Users.AnyAsync(x =>
                x.Username == request.Username || x.Email == request.Email);

            if (isExist)
                throw new Exception("Username hoặc Email đã tồn tại");
            var passwordHash = PasswordHelper.Hash(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                IsActive = true,
                IsLocked = false,
                CreatedAt = DateTime.Now
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return new RegisterResponseDTO
            {
                UserId = user.Id,
                Username = user.Username
            };
        }
        public async Task<LoginResponseDTO> RefreshTokenAsync(string refreshToken)
        {
            var oldToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(x =>
                    x.Token == refreshToken &&
                    x.RevokedAt == null &&
                    x.ExpiresAt > DateTime.Now
                );
            if (oldToken == null)
                throw new UnauthorizedAccessException("Invalid refresh token");
            oldToken.RevokedAt = DateTime.Now;
            var newRefreshToken = new RefreshToken
            {
                Id = Guid.NewGuid(),
                UserId = oldToken.UserId,
                Token = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.Now,
                ExpiresAt = DateTime.Now.AddDays(7)
            };
            _context.RefreshTokens.Add(newRefreshToken);
            var newAccessToken = _jwtHelper.GenerateToken(oldToken.UserId);

            await _context.SaveChangesAsync();

            return new LoginResponseDTO
            {
                UserId = oldToken.UserId,
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken.Token
            };
        }

    }
}
