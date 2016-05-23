
namespace MyAndromeda.Framework.Tokens
{
    public class TokenOutputContext<TModel>
        where TModel : class
    {
        public TokenContext TokenContext { get; set; }

        public TModel Model { get; set; }

        public string Output { get; set; }
    }
}