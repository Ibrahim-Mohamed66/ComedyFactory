using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Application.Extensions.ApplicationExtension;

public static class ApplicationDependencyContainer
{
    public static void AddApplicationDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddServices()
                .AddMapper()
                .AddJwtExtension(configuration);
    }
}
