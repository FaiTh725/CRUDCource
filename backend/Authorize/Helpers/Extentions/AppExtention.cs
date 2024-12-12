
using Authorize.Contracts.User;
using Authorize.Dal;
using Authorize.Helpers.Jwt;
using Authorize.Helpers.Settings;
using Authorize.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Authorize.Helpers.Extentions
{
    public static class AppExtention
    {

        public static void AddJwtService(this IServiceCollection service, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtSetting>();

            service.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = jwtSetting!.Issuer,
                        ValidAudience = jwtSetting!.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey))
                    };
                });

            service.AddAuthorization();
        }

        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBrokerSetting = configuration.GetSection("RabbitMqSetting").Get<MessageBrokerSetting>();

            services.AddMassTransit(conf =>
            {
                conf.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri($"amqp://{messageBrokerSetting!.Host}"), x =>
                    {
                        x.Username(messageBrokerSetting.Login);
                        x.Password(messageBrokerSetting.Password);

                    });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }

        public static void InitializeDatabase(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            
            var services = scope.ServiceProvider;
            var dbContext = services.GetRequiredService<AppDbContext>();

            var lastMigration = string.Empty;
            var lastApplyMigration = string.Empty;

            if (!dbContext.Database.CanConnect())
            {
                dbContext.Database.Migrate();
                
                lastMigration = dbContext.Database.GetMigrations().ToList()[^1];
                lastApplyMigration = dbContext.Database.GetAppliedMigrations().ToList()[^1];
            }
            else if (lastApplyMigration != lastMigration)
            {
                dbContext.Database.Migrate(lastApplyMigration);                
            }
        }

        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetConnectionString("RedisConnection");
                options.InstanceName = "Users";
            });
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CreateUser>, UserValidator>();
        }

        public static void AddCorses(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Frontend", policy =>
                {
                    policy.WithOrigins("http://localhost:5173")
                    .AllowCredentials()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
                });
            });
        }
    }
}
