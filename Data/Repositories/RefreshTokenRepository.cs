using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly WriteDbContext _context;
        private readonly ReadDbContext _read;
        public RefreshTokenRepository(WriteDbContext context, ReadDbContext read)
        {
            _context = context;
            _read = read;
        }
        public async Task AddRefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));

            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsRefreshTokenValidAsync(string refreshToken)
        {
            return await _read.RefreshTokens
                .WhereActive()
                .AnyAsync(rt => rt.Token == refreshToken);
        }

        public Task RevokeRefreshTokenAsync(RefreshToken refreshToken)
        {
            if (refreshToken == null)
                throw new ArgumentNullException(nameof(refreshToken));
            refreshToken.IsRevoked = true;
            refreshToken.RevokedAt = DateTime.UtcNow;
            _context.RefreshTokens.Update(refreshToken);
            return _context.SaveChangesAsync();
        }
        

        // Use it in your repository
        public async Task<bool> HasValidRefreshTokenAsync(string userId)
        {
            return await _context.RefreshTokens
                .Where(r => r.UserId == userId)
                .WhereActive()
                .AnyAsync();
        }


        public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(string userId)
        {
            return await _read.RefreshTokens
                .AsNoTracking()
                .WhereActive()
                .FirstOrDefaultAsync(rt => rt.UserId == userId);
        }
    }

}



public static class RefreshTokenExtensions
{
    public static IQueryable<RefreshToken> WhereActive(this IQueryable<RefreshToken> query)
    {
        return query.Where(r => !r.IsRevoked && r.ExpiresAt > DateTime.UtcNow);
    }
}