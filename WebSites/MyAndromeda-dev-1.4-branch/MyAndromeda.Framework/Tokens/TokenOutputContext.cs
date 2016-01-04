using MyAndromedaDataAccess.Domain.Marketing;

namespace MyAndromeda.Framework.Tokens
{
    public class TokenOutputContext
    {
        public TokenContext TokenContext { get; set; }

        public Customer Customer { get; set; }

        public string Output { get; set; }
    }
}