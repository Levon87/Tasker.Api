using Auth.Service.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.DataRepository
{
    public interface IAuthClientRepository
    {
        Task<AuthClient> GetAuthClientByName(string userName);
        Task<AuthClient> GetAuthClientByApiKey(string apeKey);
        AuthClient AddAuthClient(AuthClient authClient);
    }
}
