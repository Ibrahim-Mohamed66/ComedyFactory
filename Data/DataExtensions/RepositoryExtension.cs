using Data.Repositories;
using Data.Repositories.IRepositories;
using Microsoft.Extensions.DependencyInjection;


namespace Data.Extensions.DataExtensions;

public static class RepositoryExtension
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICountryRepository, CountryRepository>();
        services.AddScoped<ICityRepository, CityRepository>();
        services.AddScoped<IConfigurationRepository, ConfigurationRepository>();
        services.AddScoped<IBlockRepository, BlockRepository>();
        services.AddScoped<IDesireRepository, DesireRepository>();
        services.AddScoped<IMasterCategoryRepository, MasterCategoryRepository>();
        services.AddScoped<IProfessorRepository, ProfessorRepository>();
        services.AddScoped<IActivityRepository, ActivityRepository>();
        services.AddScoped<IPersonalDataRepository, PersonalDataRepository>();
        services.AddScoped<IAlbumMediaRepository, AlbumMediaRepository>();
        services.AddScoped<IAlbumRepository, AlbumRepository>();
        return services;
    }
}
