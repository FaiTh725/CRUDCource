﻿namespace Product.Helpers.Settings
{
    public class JwtConf
    {
        public string SecretKey { get; set; } = string.Empty;

        public string Issuer {  get; set; } = string.Empty;

        public string Audience {  get; set; } = string.Empty;
    }
}
