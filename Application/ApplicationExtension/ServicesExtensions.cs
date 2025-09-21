using Application.IServices;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;
using Techvally.Application.Service;

namespace Application.Extensions.ApplicationExtension
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICountryService, CountryService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IConfigurationService, ConfigurationService>();
            services.AddScoped<IBlockService, BlockService>();
            services.AddScoped<IDesireService, DesireService>();
            services.AddScoped<IMasterCategoryService, MasterCategoryService>();
            services.AddScoped<IProfessorService, ProfessorService>();
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IPersonalDataService, PersonalDataService>();
            services.AddScoped<IAlbumService, AlbumService>();
            services.AddScoped<IAlbumMediaService, AlbumMediaService>();
            return services;
        }
    }
}
