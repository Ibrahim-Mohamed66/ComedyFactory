using Admin.Extensions;
using Application.Extensions.ApplicationExtension;
using Data;
using Data.Extensions.DataExtensions;
using Microsoft.AspNetCore.Localization;
using System.Globalization;
using System.Text.Json.Serialization;


namespace Admin
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            Config.BaseURL = builder.Configuration["BaseUrls:BaseURL"];

            // Add services to the container.
            builder.Services.AddControllersWithViews()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });

            builder.Services.AddDistributedMemoryCache(); // required for session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // adjust timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });




            builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");

            // Configure request localization
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en"),
                    new CultureInfo("ar")
                };

                options.DefaultRequestCulture = new RequestCulture("en");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;

                options.RequestCultureProviders.Insert(0, new QueryStringRequestCultureProvider());
                options.RequestCultureProviders.Insert(1, new CookieRequestCultureProvider());
            });

            builder.Services.AddDataDependencies(builder.Configuration);
            builder.Services.AddApplicationDependencies(builder.Configuration);
            builder.Services.AddAdminServices();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
           
            app.UseRouting();
            app.UseRequestLocalization();
            app.UseSession();
            //app.UseRefreshTokens();
            app.UseAuthentication();
            app.UseAuthorization();
            await app.SeedDatabaseAsync();

            app.MapControllerRoute(
                name: "Blocks",
                pattern: "Blocks/{action}/{blockType}/{id?}",
                defaults: new { controller = "Blocks", action = "Index" });

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();

        }
    }
}
