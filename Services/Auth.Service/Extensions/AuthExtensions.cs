using Auth.Service.DataRepository;
using Auth.Service.Filters;
using Auth.Service.Helpers;
using Auth.Service.Model.Interfaces;
using Auth.Service.Model.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;


namespace Auth.Service.Extensions
{
    public static class AuthExtensions
    {
        public static IServiceCollection AddAuth(this IServiceCollection services, TokenAuthenticationConfiguration config, string dbConnectionString, bool isTest = false)
        {

            if (isTest)
            {
                services.AddDbContext<AuthServiceDbContext>(options =>
                      options.UseLazyLoadingProxies()
                             .EnableSensitiveDataLogging()
                             .UseInMemoryDatabase("TestingDB"));
            }
            else
            {
                services.AddDbContext<AuthServiceDbContext>(options =>
                 {
                     options.UseLazyLoadingProxies()
                         .UseSqlServer(dbConnectionString);
                 });
            }

            services.AddScoped<IUnitOfWork>(provider => new UnitOfWork(provider.GetService<AuthServiceDbContext>()));
            services.AddScoped<IAuthClientRepository>(provider => new AuthClientRepository(provider.GetService<AuthServiceDbContext>()));

            if (config.EnableTokenBlocking)
            {
                services.AddMvcCore(options =>
               {
                   options.Filters.Insert(0, new AuthorizeTokenFilter());
               });
            }

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config.SecretKey));

            services.AddSingleton(config);

            services.Configure<TokenProviderOptions>(options =>
            {
                options.Path = config.Path;
                options.Audience = config.Audience;
                options.Issuer = config.Issuer;
                options.SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
                options.Expiration = new TimeSpan(0, 0, config.Expiration);
                options.RefreshTokenSigningKey = config.SecretKey;
            });
            var tokenValidationParameters = new TokenValidationParameters
            {
                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = config.Issuer,
                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = config.Audience,
                // Validate the token expiry
                ValidateLifetime = true,
                // If you want to allow a certain amount of clock drift, set that here:
                ClockSkew = TimeSpan.Zero
            };

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o => { o.TokenValidationParameters = tokenValidationParameters; });

            services.AddSingleton<IAuthService, AuthService>();
            services.AddScoped<IAuthClientService, AuthClientService>();
            services.AddSingleton<IAuthenticationValidation, AuthenticationValidation>();

            return services;
        }

        public static IMvcBuilder AddAuthToMvc(this IMvcBuilder mvc)
        {
            var assembly = typeof(TokenProviderOptions).Assembly;

            mvc.AddApplicationPart(assembly);
            return mvc;
        }
    }
}
