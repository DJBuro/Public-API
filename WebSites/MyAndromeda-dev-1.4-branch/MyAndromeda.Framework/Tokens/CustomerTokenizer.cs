using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Framework.Tokens
{
    public class CustomerTokenizer : ITokenProvider
    {
        private static readonly Type customerType = typeof(Customer);
        //private static readonly string describeFor = "Customer";
          
        private static readonly ReplacablePart titlePart = new ReplacablePart()
        {
            Token = new Token
            {
                For = customerType.Name,
                Text = "Title",
                Value = "{Title}",
            },
            Getter = TitleGetter
        };

        private static readonly ReplacablePart firstNamePart = new ReplacablePart()
        {
            Token = new Token
            {
                For = customerType.Name,
                Text = "First Name",
                Value = "{FirstName}"
            },
            Getter = FirstNameGetter
        };

        private static readonly ReplacablePart lastNamePart = new ReplacablePart()
        {
            Token = new Token()
            {
                For = customerType.Name,
                Text = "Surname",
                Value = "{Surname}"
            },
            Getter = LastNameGetter
        };
          
        public CustomerTokenizer() 
        {
            this.State = new Dictionary<Token, ReplacablePart>();
            this.State.Add(titlePart.Token, titlePart);
            this.State.Add(firstNamePart.Token, firstNamePart);
            this.State.Add(lastNamePart.Token, lastNamePart);
        }

        private IDictionary<Token, ReplacablePart> State { get; set; }

        public IEnumerable<Token> GetTokens() 
        {
            return State.Keys;
        }

        public IEnumerable<ReplacablePart> ProcessFor<T>(TokenContext context) where T : class
        {
            var t = typeof(T);
            if (t != customerType)
                return Enumerable.Empty<ReplacablePart>();

            var tokens = this.State;
            var foundTokens = tokens.Where(token => context.Message.IndexOf(token.Key.Value, StringComparison.InvariantCultureIgnoreCase) >= 0).ToArray();

            return foundTokens.Select(e => e.Value);
        }

        private static string TitleGetter(dynamic customer)
        {
            return customer.Title;
        }

        private static string FirstNameGetter(dynamic customer)
        {
            return customer.FirstName;
        }

        private static string LastNameGetter(dynamic customer)
        {
            return customer.Surname;
        }
    }
}