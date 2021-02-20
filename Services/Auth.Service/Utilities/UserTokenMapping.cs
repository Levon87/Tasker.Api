using Auth.Service.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auth.Service.Utilities
{
    public static class UserTokenMapping
    {
        private static Dictionary<string, HashSet<UserToken>> _userTokens = new Dictionary<string, HashSet<UserToken>>();

        public static int Count
        {
            get
            {
                return _userTokens.Count;
            }
        }

        public static void Add(string key, UserToken token)
        {
            lock (_userTokens)
            {
                if (!_userTokens.TryGetValue(key, out HashSet<UserToken> userTokens))
                {
                    userTokens = new HashSet<UserToken>();
                    _userTokens.Add(key, userTokens);
                }

                lock (userTokens)
                {
                    userTokens.Add(token);
                }
            }
        }

        public static bool ExistToken(string token)
        {
            lock (_userTokens)
            {
                return _userTokens.Any(x => x.Value.Select(z => z.Token).Contains(token));
            }
        }

        public static IEnumerable<UserToken> GetTokens(string key)
        {
            if (_userTokens.TryGetValue(key, out HashSet<UserToken> tokens))
            {
                return tokens;
            }

            return Enumerable.Empty<UserToken>();
        }

        public static void Remove(string key, UserToken token)
        {
            lock (_userTokens)
            {
                if (!_userTokens.TryGetValue(key, out HashSet<UserToken> tokens))
                {
                    return;
                }

                lock (tokens)
                {
                    tokens.Remove(token);

                    if (tokens.Count == 0)
                    {
                        _userTokens.Remove(key);
                    }
                }
            }
        }

        public static void Remove(string key, bool allTokens)
        {
            lock (_userTokens)
            {
                if (allTokens)
                {
                    _userTokens.Remove(key);
                }
            }
        }

        public static void RemoveByToken(UserToken token)
        {
            lock (_userTokens)
            {
                var existToken = _userTokens.FirstOrDefault(x => x.Value.Select(z => z.Token).Contains(token.Token));

                existToken.Value.Remove(token);

                if (existToken.Value.Count <= 0)
                {
                    _userTokens.Remove(existToken.Key);
                }
            }
        }

        public static void RemoveAllExpired()
        {
            lock (_userTokens)
            {
                foreach (var userToken in _userTokens)
                {
                    userToken.Value.RemoveWhere(x => x.Expiration < DateTime.Now);

                    if (userToken.Value.Count <= 0)
                    {
                        _userTokens.Remove(userToken.Key);
                    }
                }
            }
        }
    }
}
