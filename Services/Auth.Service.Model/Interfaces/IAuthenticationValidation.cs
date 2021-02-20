using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Model.Interfaces
{
    public interface IAuthenticationValidation
    {
        Task<ClaimsIdentity> GetIdentityByLoginPair(string userName, string password);

        Task<ClaimsIdentity> GetIdentityByApiKey(string apiKey);
    }
}
