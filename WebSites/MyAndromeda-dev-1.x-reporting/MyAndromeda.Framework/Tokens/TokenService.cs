using System.Collections.Generic;
using System.Linq;
using System.Text;
using MyAndromeda.Logging;

namespace MyAndromeda.Framework.Tokens
{
    public class TokenService : ITokenService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly ITokenProvider[] tokenProviders;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenService" /> class.
        /// </summary>
        /// <param name="tokenProviders">The token providers.</param>
        /// <param name="logger">The logger.</param>
        public TokenService(ITokenProvider[] tokenProviders, IMyAndromedaLogger logger)
        { 
            this.logger = logger;
            this.tokenProviders = tokenProviders;
        }

        public IEnumerable<Token> GetAllTokens()
        {
            return tokenProviders.SelectMany(e => e.GetTokens());
        }

        public IEnumerable<TokenOutputContext<TModel>> ProcessTokens<TModel>(TokenContext context, IEnumerable<TModel> models)
            where TModel : class
        {
            var replacementContexts = new Dictionary<string, ReplacablePart[]>();

            foreach (var provider in tokenProviders)
            {
                var tokens = provider.ProcessFor<TModel>(context).ToArray();

                if (tokens.Length > 0) 
                {
                    replacementContexts.Add("Customer", tokens);
                }
            }

            foreach (var model in models) 
            {
                var sb = new StringBuilder(context.Message);
              
                foreach (var section in replacementContexts.Keys) 
                {
                    var tokens = replacementContexts[section];

                    foreach (var token in tokens) 
                    {
                        sb.Replace(token.Token.Value, token.Getter(model));   
                    }
                }

                yield return new TokenOutputContext<TModel>()
                {
                    Model = model,
                    Output = sb.ToString(),
                    TokenContext = context
                };
            }

            if (replacementContexts.Count > 0) 
            {
            }
            //foreach(var item in items)
            //{
            //    foreach (var tokenProvider in tokenProviders) 
            //    {
            //        var tokens = tokenProvider.ProcessFor(context, item);
            //        replacementContexts.Add(item, tokens);
            //    }
            //}
            //foreach (var replacementContext in replacementContexts) 
            //{
            //    this.logger.Debug("Replacing {0} tokens", replacementContext.Key);
            //    foreach(var token in replacementContext.Value)
            //    {
            //    }
            //}
        }
    }
}