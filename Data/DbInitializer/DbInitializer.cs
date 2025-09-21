using Data.Context;
using Data.DBInitializer;
using Domain.Models;
using Domain.StaticData;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Data.DBInitializer
{
    public class DbInitializer:IDbInitializer
    {
        private readonly WriteDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(WriteDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task Initialize()
        {
            try
            {
                if (_context.Database.GetPendingMigrations().Count() > 0)
                {
                    _context.Database.Migrate();
                }

            }
            catch (Exception ex)
            {

                throw;
            }
            if (!await _roleManager.RoleExistsAsync(RoleNames.User))
            {
                await _roleManager.CreateAsync(new IdentityRole(RoleNames.User));
                await _roleManager.CreateAsync(new IdentityRole(RoleNames.Admin));
                await _roleManager.CreateAsync(new IdentityRole(RoleNames.Guest));
                await _roleManager.CreateAsync(new IdentityRole(RoleNames.Trainer));
            }
            var adminUser = await _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "admin@comedy.com",
                Email = "admin@comedy.com",
                EmailConfirmed = true,
                FullName = "Admin",
                Mobile = "01234567891",
                Active = true,
                PhoneNumberConfirmed = true,
                CreatedOnUtc = DateTime.UtcNow

            }, "Admin123*");

            var user = await _userManager.FindByEmailAsync("admin@comedy.com");
            await _userManager.AddToRoleAsync(user, RoleNames.Admin);
        }
    }
}
