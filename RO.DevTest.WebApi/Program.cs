using RO.DevTest.Application;
using RO.DevTest.Infrastructure.IoC;
using RO.DevTest.Persistence.IoC;
using RO.DevTest.Application.IoC;
using RO.DevTest.WebApi.Middlewares;
using RO.DevTest.Application.Contracts.Infrastructure.Security;
using RO.DevTest.WebApi.Token;
using Microsoft.OpenApi.Models;

namespace RO.DevTest.WebApi;

public class Program {
    public static void Main(string[] args) 
    {
        const string AUTHENTICATION_TYPE = "Bearer";

        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(AUTHENTICATION_TYPE, new OpenApiSecurityScheme
            {
                Description = @"JWT Authorization header using the Bearer sheme.
                        Enter 'Bearer' [space] and then your token in the text input below.
                        Example: 'Bearer 12345abcdef'",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = AUTHENTICATION_TYPE
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = AUTHENTICATION_TYPE
                        },
                        Scheme = "oauth2",
                        Name = AUTHENTICATION_TYPE,
                        In = ParameterLocation.Header
                    },
                    new List<string>()
                }
            });
        });


        builder.Services.InjectPersistenceDependencies()
            .InjectInfrastructureDependencies(builder.Configuration)
            .InjectApplicationDependencies();

        builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

        builder.Services.AddHttpContextAccessor();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if(app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
