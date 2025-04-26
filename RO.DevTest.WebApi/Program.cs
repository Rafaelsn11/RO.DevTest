using RO.DevTest.Application;
using RO.DevTest.Infrastructure.IoC;
using RO.DevTest.Persistence.IoC;
using RO.DevTest.Application.IoC;
using RO.DevTest.WebApi.Middlewares;

namespace RO.DevTest.WebApi;

public class Program {
    public static void Main(string[] args) {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.InjectPersistenceDependencies()
            .InjectInfrastructureDependencies(builder.Configuration)
            .InjectApplicationDependencies();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if(app.Environment.IsDevelopment()) {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.UseMiddleware<ExceptionMiddleware>();

        app.MapControllers();

        app.Run();
    }
}
