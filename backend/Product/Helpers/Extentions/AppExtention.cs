﻿using Application.Contracts.SharedModels.Exceptions;
using Azure.Storage.Blobs;
using FluentValidation;
using FluentValidation.AspNetCore;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using Product.Domain.Contracts.Models.Account;
using Product.Domain.Contracts.Models.FeedBack;
using Product.Domain.Contracts.Models.Product;
using Product.Domain.Models;
using Product.Features.Consumers;
using Product.Features.Exceptions;
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
                ?? throw new AppConfigurationException("JwtSetting");
            
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

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var token = ctx.Request.Cookies["token"];
                            if (!string.IsNullOrEmpty(token))
                            {
                                ctx.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }

        public static void AddMessageBroker(this IServiceCollection services, IConfiguration configuration)
        {
            var messageBrokerSetting = configuration.GetSection("MassTransitSetting").Get<MessageBrokerConf>() 
                ?? throw new AppConfigurationException("MassTransitSetting");

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
            services.AddScoped<IValidator<FeedBackAddRequest>, AddFeedBackValidator>();
        }

        public static void AddBlobStorage(this IServiceCollection service, IConfiguration configuration)
        {
            var blobConf = configuration.GetSection("BlobStorage").Get<BlobConf>() 
                ?? throw new AppConfigurationException("BlobStorage");

            var connectionString = $"DefaultEndpointsProtocol=http;AccountName={blobConf.AccountName};" +
                $"AccountKey={blobConf.Key};" +
                $"BlobEndpoint={blobConf.BaseUrl}:{blobConf.Port}/{blobConf.AccountName};";

            service.AddSingleton(_ => new BlobServiceClient(connectionString));
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

        public static void AddCustomHttpClient(this IServiceCollection services)
        {
            services.AddHttpClient("ClientHttp")
                .ConfigurePrimaryHttpMessageHandler(_ => new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback =
                    (sender, cert, chain, sllPolicy) => { return true; }
                });

            // Use for add all http client in one place
            services.AddHttpClient("DefaultHttpClient");
        }

        public static void AddMetrics(this IServiceCollection services, 
            IConfiguration configuration, 
            WebApplicationBuilder builder)
        {
            var serviceName = configuration.GetValue<string>("Telemetry:ServiceName") 
                ?? builder.Environment.ApplicationName;

            var productCountMeter = configuration.GetValue<string>("Telemetry:ProductsMetricsName")
                ?? throw new AppConfigurationException("Telemetry:ProductsMetricsName");

            services.AddOpenTelemetry()
                .ConfigureResource(resource => resource
                    .AddService(serviceName))
                .WithMetrics(metrics => metrics
                    .AddPrometheusExporter()
                    .AddMeter(productCountMeter));
        }

        public static void AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            var redisConnectionString = configuration.GetConnectionString("RedisConnection") ??
                throw new AppConfigurationException("Connection string to redis");

            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = redisConnectionString;
                options.InstanceName = "Product";
            });
        }
    }
}
