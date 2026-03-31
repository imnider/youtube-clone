using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Services;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Interfaces.Repositories;
using YoutubeClone.Infrastructure.Persistence.SqlServer.Repositories;

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
            services.AddScoped<ErrorEventHandler>();
        }

        /// <summary>
        /// Método que añade lo esencial que necesita nuestra aplicacion para funcionar
        /// </summary>
        /// <param name="services"></param>
        public static void AddCore(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();

            services.AddSqlServer<YoutubeCloneContext>(configuration.GetConnectionString("Database"));

            // Database - Repositories
            services.AddRepositories();

            // Services
            services.AddServices();

            // Middlewares
            services.AddMiddlewares();

        }
    }
}
