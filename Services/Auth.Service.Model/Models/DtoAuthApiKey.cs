using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Model.Models
{
    public class DtoAuthApiKey
    {
        [JsonProperty(PropertyName = "apiKey")]
        public string ApiKey { get; set; }
    }
}
