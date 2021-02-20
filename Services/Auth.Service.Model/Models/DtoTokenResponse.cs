using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Model.Models
{
    public class DtoTokenResponse
    {
        [JsonProperty(PropertyName = "accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty(PropertyName = "expiresIn")]
        public int ExpiresIn { get; set; }

        [JsonProperty(PropertyName = "refreshToken")]
        public string RefreshToken { get; set; }
    }
}
