using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Model.Models
{
    public class TokenAuthenticationConfiguration
    {
        public string Path { get; set; } = "/token";

        public string Issuer { get; set; }
        public int Expiration { get; set; }

        /// <summary>
        ///     The Audience (aud) claim for the generated tokens.
        /// </summary>
        public string Audience { get; set; }

        public string SecretKey { get; set; }
        public string ApiKey { get; set; }
        public bool EnableTokenBlocking { get; set; }
    }
}
