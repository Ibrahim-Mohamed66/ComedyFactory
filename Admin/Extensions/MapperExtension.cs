using Admin.Mapper;


namespace Admin.Extensions
{
    public static class MapperExtension
    {
        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                // Admin profiles
                cfg.AddProfile<AccountMappingProfile>();
                cfg.AddProfile<CountryMappingProfile>();
                cfg.AddProfile<CityMappingProfile>();
                cfg.AddProfile<ConfigurationMappingProfile>();
                cfg.AddProfile<BlockMappingProfile>();
                cfg.AddProfile<DesireMappingProfile>();
                cfg.AddProfile<MasterCategoryMappingProfile>();
                cfg.AddProfile<ProfessorMappingProfile>();
                cfg.AddProfile<ActivityMappingProfile>();
                cfg.AddProfile<PersonalDataMappingProfile>();
                cfg.AddProfile<AlbumMappingProfile>();
                cfg.AddProfile<AlbumMediaMappingProfile>();
            });
            return services;
        }
    }
}
