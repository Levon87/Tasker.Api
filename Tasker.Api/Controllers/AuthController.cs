using System;
using System.Threading.Tasks;
using Auth.Service.Model.Interfaces;
using Auth.Service.Model.Models;
using Auth.Service.Utilities;
using Common.Core.Exceptions;
using Common.Model.Request;
using Common.Model.Response;
using Microsoft.AspNetCore.Mvc;

namespace Tasker.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/v{v:apiVersion}/auth")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(
            IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<ResultRequest<DtoTokenResponse>> Login(
            [FromBody] InboundRequest<DtoAuthLoginPair> request)
        {
            try
            {
                var dto = request?.Data;
                if (dto == null)
                    return ResultRequest<DtoTokenResponse>.Error("Access Token request fail",
                        "Invalid request data");

                var token = await _service.GenerateToken(dto.UserName, dto.Password);

                return ResultRequest<DtoTokenResponse>.Ok(token);
            }
            catch (Exception e)
            {
                return ResultRequest<DtoTokenResponse>.Error("Access Token request error",
                    e.Message);
            }
        }

        [HttpPost("login-api-key")]
        public async Task<ResultRequest<DtoTokenResponse>> LoginWithApiKey(
            [FromBody] InboundRequest<DtoAuthApiKey> request)
        {
            try
            {
                var dto = request?.Data;
                if (dto == null)
                    return ResultRequest<DtoTokenResponse>.Error("Access Token request fail",
                        "Invalid request data");

                var token = await _service.GenerateToken(dto.ApiKey);

                return ResultRequest<DtoTokenResponse>.Ok(token);
            }
            catch (Exception e)
            {
                return ResultRequest<DtoTokenResponse>.Error("Access Token request error",
                    e.Message);
            }
        }

        [HttpPost("refresh-token")]
        public ResultRequest<DtoTokenResponse> RefreshToken(
            [FromBody] InboundRequest<DtoRefreshToken> request)
        {
            try
            {
                var dto = request?.Data;
                if (dto == null)
                    return ResultRequest<DtoTokenResponse>.Error("Access Token request fail",
                        "Invalid request data");

                var encryptedRefreshToken = dto.RefreshToken;
                if (string.IsNullOrEmpty(encryptedRefreshToken))
                    throw new LogicException(
                        "Refresh Token is required for grantType RefreshToken");

                var tokenFromRefreshToken = _service.GetNewTokenFromRefreshToken(encryptedRefreshToken);

                return ResultRequest<DtoTokenResponse>.Ok(tokenFromRefreshToken);
            }
            catch (Exception e)
            {
                return ResultRequest<DtoTokenResponse>.Error("Access Token request error",
                    e.Message);
            }
        }

        [HttpPost("blockUserToken")]
        public Task<ResultRequest> BlockUserToken([FromBody]InboundRequest<DtoUserToken> request)
        {
            try
            {
                var dto = request?.Data;
                if (dto == null || (string.IsNullOrEmpty(dto.UserId) && string.IsNullOrEmpty(dto.Token)))
                {
                    return Task.FromResult(ResultRequest.Error("Access Token request fail", "Invalid request data"));
                }

                var allTokens = string.IsNullOrEmpty(dto.Token);
                if (allTokens)
                {
                    UserTokenMapping.Remove(dto.UserId, true);
                }
                else
                {
                    UserTokenMapping.Remove(dto.UserId, new UserToken
                    {
                        UserId = dto.UserId,
                        Token = dto.Token
                    });
                }

                return Task.FromResult(ResultRequest.Ok());

            }
            catch (Exception e)
            {
                return Task.FromResult(ResultRequest.Error("Access Token request error", e.Message));
            }
        }
    }
}