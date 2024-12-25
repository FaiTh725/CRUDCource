using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.Product;
using Product.Domain.Models;
using Product.Features.Consumers;
using Product.Helpers.Settings;
using Product.Validators;
using System.Text;

namespace Product.Helpers.Extentions
{
    public static class AppExtention
    {
        public static void AddJwtService(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSetting = configuration.GetSection("JwtSetting").Get<JwtConf>() 
                ?? throw new NullReferenceException("Setting jwt token is null");
            
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidAudience = jwtSetting.Audience,
                        ValidIssuer = jwtSetting.Issuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSetting.SecretKey))
                    };
                });

            services.AddAuthorization();
        }

        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBrokerSetting = configuration.GetSection("MassTransitSetting").Get<MessageBrokerConf>() 
                ?? throw new NullReferenceException("Setting mass transit is invalid");

            services.AddMassTransit(conf =>
            {
                conf.AddConsumer<AccountCreateConsumer>();

                conf.UsingRabbitMq((context, configurator) =>
                {
                configurator.Host(new Uri($"amqp://{messageBrokerSetting.Host}"), x =>
                {
                    x.Username(messageBrokerSetting.Login);
                    x.Password(messageBrokerSetting.Password);
                });

                configurator.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddValidators(this IServiceCollection services)
        {
            services.AddFluentValidationAutoValidation();
            services.AddScoped<IValidator<CreateAccount>, AccountValidator>();
            services.AddScoped<IValidator<UploadProduct>, CreateProductValidator>();
        }

        public static void AddBlobStorage(this IServiceCollection service, IConfiguration configuration)
        {
            var blobConf = configuration.GetSection("BlobStorage").Get<BlobConf>() 
                ?? throw new NullReferenceException("Blob configuration is empty");

            var connectionString = $"DefaultEndpointsProtocol=http;AccountName={blobConf.AccountName};" +
                $"AccountKey={blobConf.Key};" +
                $"BlobEndpoint=http://127.0.0.1:{blobConf.Port}/{blobConf.AccountName};";

            service.AddSingleton(_ => new BlobServiceClient(connectionString));
        }

        public static void AddCorses(this IServiceCollection service)
        {
            service.AddCors(options =>
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

        public static void AddSwaggerWithAuth(this IServiceCollection service)
        {
            service.AddSwaggerGen(setup =>
            {
                setup.SwaggerDoc("v1", new OpenApiInfo 
                { 
                    Title = "Product API", 
                    Version = "v1" 
                });
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "JWT Authentication",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Enter jwt token",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    }
                };

                setup.AddSecurityDefinition("Bearer", jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {jwtSecurityScheme, Array.Empty<string>() }
                });

            });

        }
    }
}
