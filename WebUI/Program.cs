using Application.Extensions.ApplicationExtension;
using Data;
using Data.Extensions.DataExtensions;
using IoC;
using IOC.Resources;
using Microsoft.AspNetCore.Localization.Routing;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.FileProviders;
using System.Text.Json.Serialization;
using WebUI.Mapper;

namespace WebUI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddLocalization();
            Config.BaseURL = builder.Configuration["BaseUrls:BaseURL"];

            builder.Services.AddControllersWithViews(option => option.EnableEndpointRouting = false)
                          .AddSessionStateTempDataProvider()
                          .AddJsonOptions(options =>
                          {
                              options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                              options.JsonSerializerOptions.PropertyNamingPolicy = null;
                          })
                          .AddCookieTempDataProvider()
                          .AddXmlSerializerFormatters()
                          .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                          .AddDataAnnotationsLocalization(options =>
                          {
                              options.DataAnnotationLocalizerProvider = (type, factory) =>
                              {
                                  return factory.Create(typeof(SharedResource));
                              };
                          });

            builder.Services.Configure<RouteOptions>(options =>
            {
                options.ConstraintMap.Add("language", typeof(LanguageRouteConstraint));
            });

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddDataDependencies(builder.Configuration);
            builder.Services.AddApplicationDependencies(builder.Configuration);
            builder.Services.AddWebMapper();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            var adminUploadsPath = Path.Combine(
                            Directory.GetParent(builder.Environment.ContentRootPath)!.FullName, // go one level up
                            "Admin", "wwwroot", "uploads"
                        );

            if (Directory.Exists(adminUploadsPath))
            {
                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(adminUploadsPath),
                    RequestPath = "/uploads"
                });
                }


                // ---------------------------
                // Localization configuration
                // ---------------------------
                var supportedCultures = new[] { "en", "ar" };
            var localizationOptions = new RequestLocalizationOptions()
                .SetDefaultCulture("en")
                .AddSupportedCultures(supportedCultures)
                .AddSupportedUICultures(supportedCultures);

            // Allow picking culture from route data: {language}
            localizationOptions.RequestCultureProviders.Insert(0,
                new RouteDataRequestCultureProvider()
                {
                    Options = localizationOptions,
                    RouteDataStringKey = "language",
                    UIRouteDataStringKey = "language"
                });

            app.UseRouting();
            app.UseRequestLocalization(localizationOptions);

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            // Route with language parameter - make this the primary route
            app.MapControllerRoute(
                name: "localized",
                pattern: "{language:language}/{controller=Home}/{action=Index}/{id?}");

            // Fallback route that redirects to default language
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}",
                defaults: new { language = "en" });

            app.Run();
        }
    }
}