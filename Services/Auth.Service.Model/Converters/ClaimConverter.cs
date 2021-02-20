using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace Auth.Service.Model.Converters
{
    public class ClaimConverter : JsonConverter
    {
        public override bool CanConvert(
            Type objectType)
        {
            return true; // (objectType == typeof(Claim));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jo = JObject.Load(reader);
            var dict = jo.ToObject<Dictionary<string, string>>();
            var claims = new List<Claim>();

            foreach (var key in dict.Keys)
            {
                claims.Add(new Claim(key, dict[key]));
            }

            return claims;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var claims = value as List<Claim>;
            writer.Formatting = Formatting.Indented;

            writer.WriteStartObject();
            foreach (var claim in claims)
            {
                writer.WritePropertyName(claim.Type);
                writer.WriteValue(claim.Value);
            }

            writer.WriteEndObject();
        }
    }
}
