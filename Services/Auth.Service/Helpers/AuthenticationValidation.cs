using Auth.Service.Model.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Common.Core.Exceptions;
using Common.Model.Enums;

namespace Auth.Service.Helpers
{
    public class AuthenticationValidation : IAuthenticationValidation
    {
        private IAuthClientService _authClientService;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AuthenticationValidation(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<ClaimsIdentity> GetIdentityByLoginPair(
            string userName,
            string password)
        {
            try
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    _authClientService = scope.ServiceProvider.GetRequiredService<IAuthClientService>();

                    var claims = new List<Claim>();

                    var authClient = await _authClientService.GetAuthClientByName(userName);

                    if (authClient == null || !HashHelpers.VerifyHashedPassword(authClient.PasswordHash, password))
                        throw new LogicException(ExceptionMessage.InvalidCredentials);

                    claims.Add(new Claim("role", "user"));

                    var claimIdentity = new ClaimsIdentity(new GenericIdentity(authClient.UserId.ToString()), claims);

                    return await Task.FromResult(claimIdentity);
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
        }

        public async Task<ClaimsIdentity> GetIdentityByApiKey(
            string apiKey)
        {
            try
            {
                var claims = new List<Claim>();

                //if (apiKey != _tokenAuthConfiguration.ApiKey)
                //    throw new LogicException(ExceptionMessage.InvalidCredentials);
                // TODO get apiKey from DB

                var claimIdentity = new ClaimsIdentity(new GenericIdentity("-1"), claims);
                return await Task.FromResult(claimIdentity);
            }
            catch (Exception ex)
            {
                return await Task.FromResult<ClaimsIdentity>(null);
            }
        }
    }
}
