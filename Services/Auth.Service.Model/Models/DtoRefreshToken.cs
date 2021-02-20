using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Model.Models
{
    public class DtoRefreshToken
    {
        [JsonProperty(PropertyName = "refreshToken")]
        public string RefreshToken { get; set; }
    }
}
