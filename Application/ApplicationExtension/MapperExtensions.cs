
using Application.Mapper;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions.ApplicationExtension
{
    public static class MapperExtensions
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<UserMappingProfile>();
                cfg.AddProfile<CountryMappingProfile>();
                cfg.AddProfile<CityMappingProfile>();
                cfg.AddProfile<ConfigurationMappingProfile>();
                cfg.AddProfile<BlockMappingProfile>();
                cfg.AddProfile<DesireMappingProfile>();
                cfg.AddProfile<ProfessorMappingProfile>();
                cfg.AddProfile<MasterCategoryMappingProfile>();
                cfg.AddProfile<ActivityMappingProfile>();
                cfg.AddProfile<PersonalDataMappingProfile>();
                cfg.AddProfile<AlbumMappingProfile>();
                cfg.AddProfile<AlbumMediaMappingProfile>();
            });
            return services;
        }
    }
}
