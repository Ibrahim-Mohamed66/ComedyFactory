using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extensions.DataExtensions
{
    public static class DataDependencyContainer
    {
        public static IServiceCollection AddDataDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContexts(configuration)
                    .AddRepositories()
                    .AddIdentityAuthentication();
            return services;
        }
    }
}
