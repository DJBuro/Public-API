using MyAndromeda.Core;
using MyAndromeda.Services.Ibs.Models;
using System;
using System.Collections.Concurrent;

namespace MyAndromeda.Services.Ibs
{
    public interface IIbsCacheSettings :ISingletonDependency 
    {
        Models.TokenResult GetToken(int andromedaSiteId);
    }

    public class IbsCacheSettings : IIbsCacheSettings
    {
        private readonly ConcurrentDictionary<int, TokenResult> tokens;

        public IbsCacheSettings()
        {
            this.tokens = new ConcurrentDictionary<int, Models.TokenResult>();
        }

        public TokenResult GetToken(int andromedaSiteId)
        {
            if (!tokens.ContainsKey(andromedaSiteId))
            {
                return null;
            }

            TokenResult result = null;

            if (!this.tokens.TryGetValue(andromedaSiteId, out result))
            {
                return null;
            }

            if (result.ExpiresUtc > DateTime.UtcNow.AddHours(-12))
            {
                return null;
            }

            return result;
        }

        public void AddToken(int andromedaSiteId, TokenResult tokenResult)
        {
            this.tokens.AddOrUpdate(andromedaSiteId, tokenResult, (key, model) =>
            {
                model.Token = tokenResult.Token;
                model.ExpiresUtc = tokenResult.ExpiresUtc;
                return model;
            });
        }
    }
}