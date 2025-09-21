using Data.Context;
using Data.DBInitializer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extensions.DataExtensions;

public static class DbContextExtension
{
    public static IServiceCollection AddDbContexts(this IServiceCollection services, IConfiguration configuration)
    {
        var writeConnectionString = configuration.GetConnectionString("WriteConnection");
        var readConnectionString = configuration.GetConnectionString("ReadConnection");
        services.AddDbContext<WriteDbContext>(options => options.UseNpgsql(writeConnectionString));
        services.AddDbContext<ReadDbContext>(options => options.UseNpgsql(readConnectionString));
        services.AddScoped<IDbInitializer, DbInitializer>();

        return services;
    }
}
