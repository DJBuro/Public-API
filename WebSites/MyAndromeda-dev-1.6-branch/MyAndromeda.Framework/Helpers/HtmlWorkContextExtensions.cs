using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using System;
using System.Reflection;

namespace MyAndromeda.Framework.Helpers
{
    public static class HtmlWorkContextExtensions
    {
        public static IWorkContext GetWorkContext(this HtmlHelper html) 
        {
            return DependencyResolver.Current.GetService<IWorkContext>();
        }

        public static TService GetService<TService>(this HtmlHelper html)
        {

            var service = DependencyResolver.Current.GetService<TService>();

            return service;
        }

        public static MvcHtmlString GetJsonContextOptions(this HtmlHelper html) 
        {
            var context = html.GetWorkContext();
            var result = new { 
                ChainId = context.CurrentChain.Available ? context.CurrentChain.Chain.Id.ToString() : null,
                ExternalSiteId = context.CurrentSite.Available ? context.CurrentSite.ExternalSiteId : null
            };

            var value = result.JsonNetToString(true, false).ToString();
            
            return new MvcHtmlString(value);
        }

        public static MvcHtmlString GetVersion(this HtmlHelper html) 
        {
            var wc = GetWorkContext(html);
            var v = wc.Version;
            return new MvcHtmlString(string.Format("V{0}.{1}.{2}", v.Major, v.Minor, v.Build));
        }
    }
}