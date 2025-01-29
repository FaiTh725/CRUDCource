﻿namespace Message.Helpers.Settings
{
    public class JwtConf
    {
        public string Audience { get; set; } = string.Empty;
    
        public string Issuer {  get; set; } = string.Empty;
        
        public string SecretKey {  get; set; } = string.Empty;
    }
}
