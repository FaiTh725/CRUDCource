using Application.Contracts.Response;

namespace Product.Features.Exceptions
{
    public class AplicationConfigurationException : Exception
    {
        public string ConfigurationWithError { get; set; } = string.Empty;

        public StatusCode StatusCode { get; set; }

        public AplicationConfigurationException(
            string message,
            string placeWhereException) : base(message)
        {
            StatusCode = StatusCode.InternalServerError;

            ConfigurationWithError = placeWhereException;
        }
    }
}
