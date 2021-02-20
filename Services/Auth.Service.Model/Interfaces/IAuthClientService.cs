using Auth.Service.Model.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.Model.Interfaces
{
    public interface IAuthClientService
    {
        Task AddNewAuthClient(AuthClient authClient, string password);
        Task<AuthClient> GetAuthClientByName(string userName);
        Task<AuthClient> GetAuthClientByApiKey(string apeKey);
    }
}
