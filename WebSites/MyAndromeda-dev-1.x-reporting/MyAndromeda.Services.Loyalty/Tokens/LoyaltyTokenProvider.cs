using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Tokens;

namespace MyAndromeda.Services.Loyalty.Tokens
{
    public class LoyaltyTokenProvider : ITokenProvider
    {
        private static readonly Type loyaltyType = typeof(CustomerLoyalty);

        private static readonly ReplacablePart firstNamePart = new ReplacablePart()
        {
            Token = new Token
            {
                For = loyaltyType.Name,
                Text = "Current loyalty points",
                Value = "[loyaltyPoints]"
            },
            Getter = null
        };

        private static readonly ReplacablePart loyaltyValue = new ReplacablePart()
        {
            Token = new Token
            {
                For = loyaltyType.Name,
                Text = "Current loyalty points value (£)",
                Value = "[loyaltyPointsValue]"
            },
            Getter = null
        };

        public IEnumerable<ReplacablePart> ProcessFor<T>(TokenContext context) where T : class
        {
            return Enumerable.Empty<ReplacablePart>();
        }

        public IEnumerable<Token> GetTokens()
        {
            yield return firstNamePart.Token;
            yield return loyaltyValue.Token;
        }
    }
}
