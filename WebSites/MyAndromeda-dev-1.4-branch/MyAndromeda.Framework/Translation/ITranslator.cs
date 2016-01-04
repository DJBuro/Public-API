using System.Linq;
using MyAndromeda.Core;
using System;

namespace MyAndromeda.Framework.Translation
{
    public interface ITranslator : IDependency
    {
        string T(string text);
        string T(string originalTextFormat, params object[] textParams);
    }

    public interface ITranslatorProvider : IDependency
    {
        /// <summary>
        /// Translates the text.
        /// </summary>
        /// <param name="originalText">The original text.</param>
        /// <returns></returns>
        TranslateStringContext TranslateText(string originalText);
    }

    public class Translator : ITranslator
    {
        private readonly ITranslatorProvider[] providers;

        public Translator(ITranslatorProvider[] providers)
        { 
            this.providers = providers;
        }

        public string T(string text)
        {
            TranslateStringContext result = null; 

            if (providers.Any()) 
            {
                foreach(var provider in providers)
                {
                    var query = provider.TranslateText(text);
                    if (string.IsNullOrWhiteSpace(query.TranslatedText))
                        continue;

                    result = query;
                    break;
                }
            }

            if (result == null) { return text; }

            return result.TranslatedText;
        }

        public string T(string originalTextFormat, params object[] textParams)
        {
            var format = this.T(originalTextFormat);

            return string.Format(format, textParams);
        }
    }

    public class TranslateStringContext
    {
        public string OriginalText { get; set; }
        public string TranslatedText { get; set; }
    }
}
