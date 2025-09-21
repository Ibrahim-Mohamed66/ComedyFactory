using Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace Application.Extensions.ApplicationExtension
{
    public static class JwtExtensions
    {
        public static IServiceCollection AddJwtExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtConfig = configuration.GetSection("Jwt").Get<JWT>();
            services.Configure<JWT>(configuration.GetSection("Jwt"));

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true, // ensure access token expiry is respected
                    ValidateIssuerSigningKey = true,
                    ClockSkew = TimeSpan.Zero, // no extra leeway
                    ValidIssuer = jwtConfig.Issuer,
                    ValidAudience = jwtConfig.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtConfig.SecretKey)
                    )
                };

                options.Events = new JwtBearerEvents
                {
                    // 🔹 Check Session if Authorization header is empty
                    OnMessageReceived = context =>
                    {
                        if (string.IsNullOrEmpty(context.Token))
                        {
                            var token = context.HttpContext.Session.GetString("AccessToken");
                            if (!string.IsNullOrEmpty(token))
                            {
                                context.Token = token;
                            }
                        }
                        return Task.CompletedTask;
                    },

                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception is SecurityTokenExpiredException)
                        {
                            context.Response.Headers["Token-Expired"] = "true";
                        }
                        return Task.CompletedTask;
                    },

                    // 🔹 Handle unauthorized requests
                    OnChallenge = context =>
                    {
                        context.HandleResponse();

                        // If it's API or refresh endpoint → return 401 JSON
                        if (context.Request.Path.StartsWithSegments("/api") ||
                            context.Request.Path.StartsWithSegments("/Account/Refresh"))
                        {
                            context.Response.StatusCode = 401;

                            if (context.AuthenticateFailure is SecurityTokenExpiredException)
                            {
                                context.Response.Headers["Token-Expired"] = "true";
                            }

                            return context.Response.WriteAsJsonAsync(new
                            {
                                message = "Unauthorized"
                            });
                        }

                        // Else (MVC pages) → redirect to login
                        context.Response.Redirect(
                            "/Account/Login?ReturnUrl=" + WebUtility.UrlEncode(context.Request.Path.ToString())
                        );
                        return Task.CompletedTask;
                    }
                };
            });

            return services;
        }
    }
}
