using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class UserReadRepository : IUserReadRepository
{
    private readonly ReadDbContext _context;

    public UserReadRepository(ReadDbContext context)
    {
        _context = context;
    }

    public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
     => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);

    public async Task<ApplicationUser?> GetUserByIdAsync(string id) 
        => await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);

    public async Task<bool> IsEmailExistAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
