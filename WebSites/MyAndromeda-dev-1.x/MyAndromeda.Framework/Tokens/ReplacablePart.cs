using System;

namespace MyAndromeda.Framework.Tokens
{
    public class ReplacablePart
    {
        public Token Token { get; set; }

        public Func<dynamic, string> Getter { get; set; }
    }
}