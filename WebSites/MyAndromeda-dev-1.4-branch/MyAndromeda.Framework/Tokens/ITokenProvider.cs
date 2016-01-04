using System.Collections.Generic;
using MyAndromeda.Core;

namespace MyAndromeda.Framework.Tokens
{
    public interface ITokenProvider : IDependency
    {
        /// <summary>
        /// Processes for.
        /// </summary>
        /// <typeparam name="T">The type of the T.</typeparam>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        IEnumerable<ReplacablePart> ProcessFor<T>(TokenContext context) where T : class;

        /// <summary>
        /// Gets the tokens.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Token> GetTokens();
    }
}