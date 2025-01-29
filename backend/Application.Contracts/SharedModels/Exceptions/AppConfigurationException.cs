namespace Application.Contracts.SharedModels.Exceptions
{
    public class AppConfigurationException : Exception
    {
        public string ConfigurationErrorSection { get; init; } = string.Empty;

        public AppConfigurationException(
            string configurationSection,
            string message=""): base(message)
        {
            ConfigurationErrorSection = configurationSection;
        }
    }
}
