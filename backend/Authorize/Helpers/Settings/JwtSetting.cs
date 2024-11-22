namespace Authorize.Helpers.Jwt
{
    public class JwtSetting
    {
        public string Audience { get; set; } = string.Empty;

        public string Issuer {  get; set; } = string.Empty;

        public string SecretKey { get; set; } = string.Empty;
    }
}
