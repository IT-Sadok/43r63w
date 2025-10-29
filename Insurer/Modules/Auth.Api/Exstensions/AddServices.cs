using Auth.Api.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.Api.Exstensions;

public static class Extensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<AuthService>();
        services.AddScoped<JwtTokenGenerator>();
        services.AddHttpContextAccessor();
        services.AddScoped<IUserContextAccessor, IUserContextAccessor>();
        return services;
    }

    public static IServiceCollection AddAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
          .AddJwtBearer(options =>
           {
               options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
               {
                   ValidateAudience = true,
                   ValidateIssuer = true,
                   RequireExpirationTime = true,
                   ValidateIssuerSigningKey = true,
                   ValidIssuer = configuration.GetValue<string>("Jwt:Issuer"),
                   ValidAudience = configuration.GetValue<string>("Jwt:Audience"),
                   IssuerSigningKey = new SymmetricSecurityKey(
                       Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
               };
           });
        return services;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddDbContext<AuthDbContext>(e =>
        {
            e.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddIdentityCore<IdentityUser>()
            .AddEntityFrameworkStores<AuthDbContext>();

        return services;
    }
}

