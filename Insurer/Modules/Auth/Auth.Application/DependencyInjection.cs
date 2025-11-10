using System.Text;
using Auth.Application.Contracts;
using Auth.Application.Options;
using Auth.Application.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace Auth.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddAuthApplication(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                var key = configuration.GetValue<string>("Jwt:Key");
                var issuer = configuration.GetValue<string>("Jwt:Issuer");
                var audience = configuration.GetValue<string>("Jwt:Audience");
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    ValidateIssuer = true,
                    ValidateAudience = true
                };
            });

        services.AddAuthorization();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IAuthServicePublic, AuthServicePublic>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        services.AddOptions<JwtOptions>()
            .Bind(configuration.GetSection("Jwt")); 
        return services;
    }
}