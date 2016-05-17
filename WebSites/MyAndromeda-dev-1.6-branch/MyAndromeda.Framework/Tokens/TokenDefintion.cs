namespace MyAndromeda.Framework.Tokens
{
    public class Token
    {
        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        /// <value>The text.</value>
        public virtual string Text { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>The value.</value>
        public virtual string Value { get; set; }
        
        /// <summary>
        /// Gets or sets what the token is used for.
        /// </summary>
        /// <value>For.</value>
        public virtual string For { get; set; }
    }

    public class TokenDefintion : Token
    {
        private readonly Token token;

        public TokenDefintion(Token token) 
        {
            this.token = token;
        }

        public override string For
        {
            get { return token.For; }
            set { token.For = value; }
        } 

        public override string Value
        {
            get
            {
                return string.Format("{0}", token.Value);
            }
            set 
            {
                token.Value = value;    
            }
        }

        public override string Text
        {
            get
            {
                return this.token.Text;
            }
            set
            {
                this.token.Text = value; 
            }
        }
        
    }

}