using Domain.Models;


namespace Data.Repositories.IRepositories
{
    public interface IRefreshTokenRepository
    {
        Task<bool> IsRefreshTokenValidAsync(string refreshToken);
        Task AddRefreshTokenAsync(RefreshToken refreshToken);
        Task RevokeRefreshTokenAsync(RefreshToken refreshToken);
        Task<bool> HasValidRefreshTokenAsync(string userId);
        Task<RefreshToken?> GetRefreshTokenByUserIdAsync(string UserId);

    }
}
