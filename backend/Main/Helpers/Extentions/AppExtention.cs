using Application.Contracts.SharedModels.Exceptions;
using System.Net.Security;

namespace Main.Helpers.Extentions
{
    public static class AppExtention
    {
        public static void AddCustomReverseProxy(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddReverseProxy()
                .LoadFromConfig(configuration.GetSection("ReverseProxy"))
                .ConfigureHttpClient((contex, handler) =>
                {
                    handler.SslOptions.RemoteCertificateValidationCallback =
                    new RemoteCertificateValidationCallback((sender, certificate, chain, policyError) =>
                    {
                        return true;
                    });
                });
        }

        public static void AddCorsExt(this IServiceCollection serivce, IConfiguration configuration)
        {
            var frontendApi = configuration
                .GetValue<string>("ApiList:FrontendHttp") ?? 
                throw new AppConfigurationException("ApiList Frontend");

            var frontendApiHttps = configuration
                .GetValue<string>("ApiList:FrontendHttps") ??
                throw new AppConfigurationException("ApiList Frontend");

            serivce.AddCors(options => options.AddPolicy(
                "Frondend", 
                options =>
                {
                    options
                    .WithOrigins(frontendApi, frontendApiHttps)
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
                }));
        }
    }
}
