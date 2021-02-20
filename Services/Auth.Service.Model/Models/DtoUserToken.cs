using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Model.Models
{
    public class DtoUserToken
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "token")]
        public string Token { get; set; }
    }
}
