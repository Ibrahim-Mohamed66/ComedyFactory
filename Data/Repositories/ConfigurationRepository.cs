using Data.Context;
using Data.Repositories.IRepositories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ConfigurationRepository : IConfigurationRepository
    {
        private readonly WriteDbContext _context;
        public ConfigurationRepository(WriteDbContext context)
        {
            _context = context;

        }

        public async Task<bool> AddConfigurationAsync(Configuration configuration)
        {
            await _context.Configurations.AddAsync(configuration);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AnyAsync()
        {
           return await _context.Configurations.AnyAsync();
        }

        public async Task<Configuration> GetFirstOrDefaultAsync(Expression<Func<Configuration, bool>>? filter = null)
        {
            IQueryable<Configuration> query = _context.Configurations;
            if (filter != null)
                query = query.Where(filter);

            return await query.FirstOrDefaultAsync();
        }

        public async Task<Configuration?> GetConfigurationAsync(int id)
        {
            var configuration = await _context.Configurations.FindAsync(id);
            return configuration;
        }
        public async Task<Configuration?> GetFirstConfigurationAsync()
        {
            return await _context.Configurations.FirstOrDefaultAsync();
        }
        public async Task<bool> UpdateConfiguration(Configuration configuration)
        {
            _context.Configurations.Update(configuration);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
