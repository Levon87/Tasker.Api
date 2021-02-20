using Auth.Service.DataRepository;
using Auth.Service.Helpers;
using Auth.Service.Model.Entities;
using Auth.Service.Model.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auth.Service
{
    public class AuthClientService : IAuthClientService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthClientService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddNewAuthClient(AuthClient authClient, string password)
        {
            AuthClient existingUser;

            if (string.IsNullOrEmpty(authClient.UserName))
            {
                existingUser = await _unitOfWork.AuthClients.GetAuthClientByApiKey(authClient.ApiKey);
            }
            else
            {
                existingUser = await _unitOfWork.AuthClients.GetAuthClientByName(authClient.UserName);
            }


            if (existingUser != null)
            {
                if (string.IsNullOrEmpty(authClient.UserName))
                {
                    throw new InvalidOperationException($"User {authClient.UserName} already exists");
                }
                else
                {
                    throw new InvalidOperationException($"User {authClient.UserName} already exists");
                }
            }

            var hashedPassword = HashHelpers.HashPassword(password);

            authClient.RegisterDate = DateTime.UtcNow;
            authClient.PasswordHash = hashedPassword;
            authClient.Id = Guid.NewGuid();

            _unitOfWork.AuthClients.AddAuthClient(authClient);
            _unitOfWork.Complete();

        }

        public async Task<AuthClient> GetAuthClientByName(string userName)
        {
            return await _unitOfWork.AuthClients.GetAuthClientByName(userName);
        }

        public async Task<AuthClient> GetAuthClientByApiKey(string apeKey)
        {
            return await _unitOfWork.AuthClients.GetAuthClientByApiKey(apeKey);
        }
    }
}
