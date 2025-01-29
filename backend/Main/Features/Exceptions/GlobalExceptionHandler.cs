using Application.Contracts.SharedModels.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Main.Features.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly IHostApplicationLifetime application;
        private readonly ILogger<GlobalExceptionHandler> logger;

        public GlobalExceptionHandler(
            IHostApplicationLifetime application,
            ILogger<GlobalExceptionHandler> logger)
        {
            this.application = application;
            this.logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Instance = "API",
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "API Error"
            };

            if (exception is AppConfigurationException aplicationEx)
            {
                logger.LogError("Error configure app, section with error - " + aplicationEx.ConfigurationErrorSection);

                application.StopApplication();
            }
            else
            {
                problemDetails.Detail = "Eror API";
                problemDetails.Type = "Unknown Server Eror";
            }

            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails);


            return true;
        }
    }
}
