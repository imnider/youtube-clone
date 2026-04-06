using Microsoft.AspNetCore.Mvc;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Services;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Interfaces.Repositories;
using YoutubeClone.Infrastructure.Persistence.SqlServer.Repositories;
using YoutubeClone.Shared.Constants;
using YoutubeClone.WebApp.Middlewares;

namespace YoutubeClone.WebApp.Extensions
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Método que sirve para añadir todos los servicios de la aplicacion
        /// </summary>
        /// <param name="services"></param>
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
        }

        /// <summary>
        /// Método que sirve para añadir todos los respositorios de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddTransient<IUserRepository, UserRepository>();
        }

        /// <summary>
        /// Método que sirve para añadir todos los middlewares
        /// </summary>
        /// <param name="services"></param>
        public static void AddMiddlewares(this IServiceCollection services)
        {
            services.AddScoped<ErrorHandleMiddleware>();
        }

        public static void AddLogging(this IServiceCollection services)
        {
            services.AddSerilog();

            Log.Logger = new LoggerConfiguration()
                .WriteTo
                .MSSqlServer(
                    connectionString: "Server=localhost,1433;User=sa;Password=Admin1234@;Database=YoutubeClone;TrustServerCertificate=True;",
                    sinkOptions: new MSSqlServerSinkOptions
                    {
                        TableName = "LogEvents",
                        AutoCreateSqlTable = true
                    })
                .WriteTo.Console()
                .WriteTo.File(Path.Combine(Directory.GetCurrentDirectory(), "logs", "log.txt"), rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }

        /// <summary>
        /// Método que añade lo esencial que necesita nuestra aplicacion para funcionar
        /// </summary>
        /// <param name="services"></param>
        public static void AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers().ConfigureApiBehaviorOptions(option =>
            {
                option.InvalidModelStateResponseFactory = (errorContext) =>
                {
                    var errors = errorContext.ModelState.Values.SelectMany(value => value.Errors.Select(error => error.ErrorMessage).ToList()).ToList();
                    var response = ResponsesHelper.Create(
                        data: ValidationConstants.VALIDATION_MESSAGE,
                        errors: errors,
                        message: ValidationConstants.VALIDATION_MESSAGE);
                    return new BadRequestObjectResult(response);
                };
            });
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            services.AddSqlServer<YoutubeCloneContext>(configuration.GetConnectionString("Database"));

            // Database - Repositories
            services.AddRepositories();

            // Services
            services.AddServices();

            // Middlewares
            services.AddMiddlewares();

            AddLogging(services);
        }
    }
}
