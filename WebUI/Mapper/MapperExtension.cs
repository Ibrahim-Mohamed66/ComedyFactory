namespace WebUI.Mapper
{
    public static class MapperExtension
    {
        public static IServiceCollection AddWebMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<PersonalDataMappingProfile>();
                cfg.AddProfile<AccountMappingProfile>();
            });
            return services;
        }
    }
}
