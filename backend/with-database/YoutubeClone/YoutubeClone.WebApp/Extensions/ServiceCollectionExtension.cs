using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using YoutubeClone.Application.Helpers;
using YoutubeClone.Application.Interfaces.Services;
using YoutubeClone.Application.Services;
using YoutubeClone.Domain.Database;
using YoutubeClone.Domain.Database.SqlServer.Context;
using YoutubeClone.Domain.Exceptions;
using YoutubeClone.Domain.Interfaces.Repositories;
using YoutubeClone.Infrastructure.Persistence.SqlServer;
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
            services.AddScoped<ICacheService, CacheService>();
            services.AddScoped<IAuthServices, AuthService>();

            services.AddScoped<IUserService, UserService>();
        }

        /// <summary>
        /// Método que sirve para añadir todos los respositorios de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddRepositories(this IServiceCollection services)
        {

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserRepository, UserRepository>();
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
        /// Método para inicializar el primer usuario de la aplicación
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public async static Task Initialize(this IServiceCollection services)
        {
            var provider = services.BuildServiceProvider();
            var scope = provider.CreateAsyncScope();

            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            await userService.CreateFirstUser();
        }

        public static void AddAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(builder =>
            {
                builder.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                builder.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(builder =>
            {
                var tokenConfiguration = TokenHelper.Configuration(configuration);

                builder.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = tokenConfiguration.Issuer,
                    ValidateAudience = true,
                    ValidAudience = tokenConfiguration.Audience,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = tokenConfiguration.SecurityKey,
                    ClockSkew = TimeSpan.Zero
                };

                builder.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        throw new UnauthorizedException(ResponseConstants.AUTH_TOKEN_NOT_FOUND);
                    }
                };
            });

            services.AddAuthorization();
        }

        /// <summary>
        /// Método que agrega el caché de la aplicación
        /// </summary>
        /// <param name="services"></param>
        public static void AddCache(this IServiceCollection services)
        {
            services.AddMemoryCache();
        }

        /// <summary>
        /// Método que agrega todoa la extensión
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configuration"></param>
        public static async Task AddCore(this IServiceCollection services, IConfiguration configuration)
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

            services.AddOpenApi();

            services.AddSqlServer<YoutubeCloneContext>(configuration.GetConnectionString("Database"));

            // Database - Repositories
            services.AddRepositories();

            // Services
            services.AddServices();

            // Middlewares
            services.AddMiddlewares();

            // Serilog
            services.AddLogging();

            //Cache
            services.AddCache();

            // Auth
            services.AddAuth(configuration);

            // Inicializar primer usuario
            await Initialize(services);
        }
    }
}
