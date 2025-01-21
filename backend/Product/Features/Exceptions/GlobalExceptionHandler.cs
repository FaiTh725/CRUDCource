using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Product.Features.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
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

            if(exception is AplicationConfigurationException aplicationEx)
            {
                problemDetails.Detail = aplicationEx.Message;
                problemDetails.Type = "Error with configuration " + 
                    aplicationEx.ConfigurationWithError;
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
