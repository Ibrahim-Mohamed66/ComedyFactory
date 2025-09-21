using Application.DTOS;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;



namespace Application.IServices
{
    public interface IUserService
    {
        Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
        Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
        Task<AuthResponseDto> GetRefreshToken(RefreshTokenRequestDto requestDto);
        TokenValidationParameters GetValidationParameters(bool validateLifetime = true);
        bool IsTokenExpired(string token);
        Task<bool> IsEmailExsitstAsync(string email);
    }
}
