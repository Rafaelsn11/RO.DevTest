using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RO.DevTest.Application.Contracts.Infrastructure;
using RO.DevTest.Application.Contracts.Infrastructure.Security;
using RO.DevTest.Domain.Entities;
using RO.DevTest.Infrastructure.Abstractions;
using RO.DevTest.Infrastructure.Security.Token.Access.Generator;
using RO.DevTest.Infrastructure.Security.Token.Refresh;
using RO.DevTest.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using RO.DevTest.Application.Contracts.Infrastructure.Services.LoggedUser;
using RO.DevTest.Infrastructure.Services.LoggedUser;
namespace RO.DevTest.Infrastructure.IoC;

/// <summary>
/// Provides dependency injection configuration for the Infrastructure layer.
/// This class is responsible for registering all infrastructure services,
/// including identity, security, and token-related services.
/// </summary>
public static class InfrastructureDependecyInjector 
{
    /// <summary>
    /// Injects all dependencies of the Infrastructure layer into the provided
    /// <see cref="IServiceCollection"/>. This includes identity services,
    /// security services, and token-related services.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to inject the dependencies into
    /// </param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> containing application settings
    /// </param>
    /// <returns>
    /// The <see cref="IServiceCollection"/> with all infrastructure dependencies injected
    /// </returns>
    public static IServiceCollection InjectInfrastructureDependencies(
        this IServiceCollection services, 
        IConfiguration configuration) 
    {
        AddIdentityServices(services);
        AddSecurityServices(services, configuration);
        AddJwtAuthentication(services, configuration);

        return services;
    }

    /// <summary>
    /// Configures and adds identity-related services to the service collection.
    /// This includes user identity, roles, and token providers.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add identity services to
    /// </param>
    private static void AddIdentityServices(IServiceCollection services)
    {
        services.AddDefaultIdentity<User>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<DefaultContext>()
            .AddDefaultTokenProviders();

        services.AddScoped<IIdentityAbstractor, IdentityAbstractor>();
    }

    /// <summary>
    /// Configures and adds security-related services to the service collection.
    /// This includes JWT token generation and validation services.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add security services to
    /// </param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> containing JWT settings
    /// </param>
    private static void AddSecurityServices(IServiceCollection services, IConfiguration configuration)
    {
        var expirationTimeMinutes = configuration.GetValue<uint>("Settings:Jwt:ExpirationTimeMinutes");
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        var identityAbstractor = services.BuildServiceProvider().GetRequiredService<IIdentityAbstractor>();
        services.AddScoped<IAccessTokenGenerator>(_ => 
            new JwtTokenGenerator(expirationTimeMinutes, signingKey!, identityAbstractor));

        services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();

        services.AddScoped<ILoggedUser, LoggedUser>();
    }

    /// <summary>
    /// Configures JWT authentication and authorization services.
    /// Sets up the JWT Bearer authentication scheme with custom validation parameters
    /// and defines authorization policies based on user roles.
    /// </summary>
    /// <param name="services">
    /// The <see cref="IServiceCollection"/> to add authentication services to
    /// </param>
    /// <param name="configuration">
    /// The <see cref="IConfiguration"/> containing JWT settings
    /// </param>
    private static void AddJwtAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        var signingKey = configuration.GetValue<string>("Settings:Jwt:SigningKey");

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(signingKey!)),
                ClockSkew = new TimeSpan(0),
            };
        });

        services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            options.AddPolicy("CustomerOnly", policy => policy.RequireRole("Customer"));
            options.AddPolicy("AdminOrCustomer", policy => policy.RequireRole("Admin", "Customer"));
        });
    }
}
