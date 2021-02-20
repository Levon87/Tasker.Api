using Auth.Service.Model.Converters;
using Auth.Service.Model.Interfaces;
using Auth.Service.Model.Models;
using Auth.Service.Model.Utilities;
using Auth.Service.Utilities;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Claims;
using System.Threading.Tasks;
using Common.Core.Exceptions;
using Common.Model.Enums;

namespace Auth.Service
{
    public class AuthService : IAuthService
    {
        private readonly IAuthenticationValidation _authenticationValidation;
        private readonly TokenProviderOptions _tokenProviderOptions;

        public AuthService(IOptions<TokenProviderOptions> options, IAuthenticationValidation authenticationValidation)
        {
            _authenticationValidation = authenticationValidation;
            _tokenProviderOptions = options.Value;
            ThrowIfInvalidOptions(_tokenProviderOptions);
        }

        public async Task<DtoTokenResponse> GenerateToken(string userName, string password)
        {
            var identity = await _authenticationValidation.GetIdentityByLoginPair(userName, password);

            if (identity == null)
                throw new LogicException(ExceptionMessage.InvalidCredentials);

            var now = DateTime.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime()
                    .ToUnixTimeSeconds()
                    .ToString(), ClaimValueTypes.Integer64)
            };
            claims.AddRange(identity.Claims);

            var encodedJwt = GetJwt(claims, now);

            UserTokenMapping.RemoveAllExpired();
            UserTokenMapping.Add(identity.Name, new UserToken
            {
                UserId = identity.Name,
                Token = encodedJwt,
                Expiration = now.Add(_tokenProviderOptions.Expiration)
            });

            var encryptedRefreshToken = GetRefreshToken(claims, identity.Name);

            var response = new DtoTokenResponse
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_tokenProviderOptions.Expiration.TotalSeconds,
                RefreshToken = encryptedRefreshToken
            };

            return response;
        }

        public async Task<DtoTokenResponse> GenerateToken(string apiKey)
        {
            var identity = await _authenticationValidation.GetIdentityByApiKey(apiKey);
            if (identity == null)
                throw new LogicException(ExceptionMessage.InvalidCredentials);

            var now = DateTime.UtcNow;

            // Specifically add the jti (nonce), iat (issued timestamp), and sub (subject/user) claims.
            // You can add other claims here, if you want:

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, identity.Name),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime()
                    .ToUnixTimeSeconds()
                    .ToString(), ClaimValueTypes.Integer64)
            };
            claims.AddRange(identity.Claims);

            var encodedJwt = GetJwt(claims, now);

            var encryptedRefreshToken = GetRefreshToken(claims, identity.Name);

            var response = new DtoTokenResponse
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_tokenProviderOptions.Expiration.TotalSeconds,
                RefreshToken = encryptedRefreshToken
            };

            return response;
        }

        public DtoTokenResponse GetNewTokenFromRefreshToken(string tokenEncrypted)
        {
            var token = CryptoUtility.Decrypt(tokenEncrypted, _tokenProviderOptions.RefreshTokenSigningKey);

            var data = Convert.FromBase64String(token);
            var obj = ByteArrayToObject(data) as RefreshTokenPayloadModel;
            var claims = JsonConvert.DeserializeObject<IEnumerable<Claim>>(obj.Claims, new ClaimConverter());

            var when = DateTime.FromBinary(BitConverter.ToInt64(obj.TimeWithKey, 0));
            if (when < DateTime.UtcNow.AddHours(-4))
                throw new LogicException(ExceptionMessage.ExpiredRefreshToken);

            var userId = claims.First(x => x.Type == "sub")
                .Value;

            var encodedJwt = GetJwt(claims, DateTime.UtcNow);
            if ((DateTime.UtcNow - when).Hours > 3)
                tokenEncrypted = GetRefreshToken(claims, userId);

            return new DtoTokenResponse
            {
                AccessToken = encodedJwt,
                ExpiresIn = (int)_tokenProviderOptions.Expiration.TotalSeconds,
                RefreshToken = tokenEncrypted
            };
        }

        private string GetJwt(IEnumerable<Claim> claims, DateTime now)
        {
            // Create the JWT and write it to a string
            var jwt = new JwtSecurityToken(
                _tokenProviderOptions.Issuer,
                _tokenProviderOptions.Audience,
                claims,
                now,
                now.Add(_tokenProviderOptions.Expiration),
                _tokenProviderOptions.SigningCredentials);

            return new JwtSecurityTokenHandler().WriteToken(jwt);
        }

        private string GetRefreshToken(
            IEnumerable<Claim> claims,
            string userId)
        {
            var time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            var key = CryptoUtility.CreateCryptographicallySecureGuid()
                .ToByteArray();

            var timeKey = time.Concat(key)
                .ToArray();
            var serializedClaims = JsonConvert.SerializeObject(claims, new ClaimConverter());
            var payload = new RefreshTokenPayloadModel
            {
                UserId = userId,
                Claims = serializedClaims,
                TimeWithKey = timeKey
            };
            var payloadByteArr = ObjectToByteArray(payload);
            var refreshToken = Convert.ToBase64String(payloadByteArr);
            var encryptedRefreshToken = CryptoUtility.Encrypt(refreshToken,
                _tokenProviderOptions.RefreshTokenSigningKey);

            return encryptedRefreshToken;
        }

        private static void ThrowIfInvalidOptions(TokenProviderOptions options)
        {
            if (string.IsNullOrEmpty(options.Path))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Path));

            if (string.IsNullOrEmpty(options.Issuer))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Issuer));

            if (string.IsNullOrEmpty(options.Audience))
                throw new ArgumentNullException(nameof(TokenProviderOptions.Audience));

            if (options.Expiration == TimeSpan.Zero)
                throw new ArgumentException("Must be a non-zero TimeSpan.",
                    nameof(TokenProviderOptions.Expiration));

            if (options.SigningCredentials == null)
                throw new ArgumentNullException(nameof(TokenProviderOptions.SigningCredentials));
        }

        private byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return new byte[0];
            var bf = new BinaryFormatter();
            using (var ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }

        // Convert a byte array to an Object
        private object ByteArrayToObject(byte[] arrBytes)
        {
            using (var memStream = new MemoryStream())
            {
                var binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var obj = binForm.Deserialize(memStream);

                return obj;
            }
        }
    }
}
