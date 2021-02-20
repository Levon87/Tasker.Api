using Auth.Service.Model.Models;
using System.Threading.Tasks;

namespace Auth.Service.Model.Interfaces
{
    public interface IAuthService
    {
        Task<DtoTokenResponse> GenerateToken(string userName, string password);
        Task<DtoTokenResponse> GenerateToken(string apiKey);
        DtoTokenResponse GetNewTokenFromRefreshToken(string tokenEncrypted);
    }
}
