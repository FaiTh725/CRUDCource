using Message.Helpers.Settings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Message.Helpers.Extentiosn
{
    public static class AppExtentions
    {
        public static void AddRedis(this IServiceCollection services, IConfiguration configurations)
        {
            services.AddSingleton<IConnectionMultiplexer>(options =>
                {
                    var redisOptions = ConfigurationOptions.Parse(configurations.GetConnectionString("RedisConnection"));
                    redisOptions.AbortOnConnectFail = false;

                    return ConnectionMultiplexer.Connect(redisOptions);
                }
            );
        }

        public static void AddCorsExt(this IServiceCollection service, IConfiguration configuration)
        {
            var frontendApi = configuration.GetValue<string>("ApiList:Frontend");
            
            service.AddCors(options =>
            {
                options.AddPolicy("Frontend", 
                    options =>
                    {
                        options.WithOrigins(frontendApi!)
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
        }

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

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = ctx =>
                        {
                            var token = ctx.Request.Cookies["token"];
                            if(!string.IsNullOrEmpty(token))
                            {
                                ctx.Token = token;
                            }

                            return Task.CompletedTask;
                        }
                    };
                });

            services.AddAuthorization();
        }
    }
}
