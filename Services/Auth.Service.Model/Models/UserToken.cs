using System;
using System.Collections.Generic;
using System.Text;

namespace Auth.Service.Model.Models
{
    public class UserToken
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }

        public override bool Equals(object obj)
        {
            var item = obj as UserToken;

            return Token.Equals(item.Token);
        }

        public override int GetHashCode()
        {
            return Token.GetHashCode();
        }
    }
}
