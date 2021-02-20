using System;
using Newtonsoft.Json;

namespace Common.Model.Model
{
    public class ModelBase
    {
        [JsonProperty(PropertyName = "id")]
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}