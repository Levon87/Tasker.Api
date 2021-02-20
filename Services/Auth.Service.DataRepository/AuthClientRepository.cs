using Auth.Service.Model.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service.DataRepository
{
    public class AuthClientRepository : Repository<AuthClient>, IAuthClientRepository
    {
        public AuthClientRepository(DbContext context) : base(context)
        {
        }

        public async Task<AuthClient> GetAuthClientByName(string userName)
        {
            var clients = await FindAsync(x => x.UserName == userName);

            return clients.FirstOrDefault();
        }

        public async Task<AuthClient> GetAuthClientByApiKey(string apeKey)
        {
            var clients = await FindAsync(x => x.ApiKey == apeKey);

            return clients.FirstOrDefault();
        }

        public AuthClient AddAuthClient(AuthClient authClient)
        {
            return Add(authClient);
        }
    }
}
