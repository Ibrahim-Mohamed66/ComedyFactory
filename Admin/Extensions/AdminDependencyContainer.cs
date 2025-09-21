namespace Admin.Extensions
{
    public static class AdminDependencyContainer
    {
        public static IServiceCollection AddAdminServices(this IServiceCollection services)
        {
            services.AddMapper();
            return services;
        }
    }
}
