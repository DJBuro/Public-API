using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Core;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Framework.Tokens
{
    public interface ITokenService : IDependency 
    {
        /// <summary>
        /// Todo: make a better one
        /// Processes the tokens.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="customers">The customers.</param>
        /// <returns></returns>
        IEnumerable<TokenOutputContext> ProcessTokens(TokenContext context, IEnumerable<Customer> customers);

        /// <summary>
        /// Gets all the tokens provided by all the Token Providers.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Token> GetAllTokens();
    }
}
