using Domain.Models;

using System.Linq.Expressions;


namespace Data.Repositories.IRepositories
{
    public interface IConfigurationRepository
    {
        public Task<bool> AddConfigurationAsync(Configuration configuration);
        public Task<Configuration?> GetConfigurationAsync(int id);
        public Task<bool> UpdateConfiguration(Configuration Configuration);
        public Task<Configuration> GetFirstOrDefaultAsync(Expression<Func<Configuration, bool>>? filter = null);
        public Task<bool> AnyAsync();
        Task<Configuration?> GetFirstConfigurationAsync();
    }
}
