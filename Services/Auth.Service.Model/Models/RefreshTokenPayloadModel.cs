using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Model.Models
{
    [Serializable]
    public class RefreshTokenPayloadModel
    {
        public string Claims { get; set; }
        public string UserId { get; set; }
        public byte[] TimeWithKey { get; set; }
    }
}
