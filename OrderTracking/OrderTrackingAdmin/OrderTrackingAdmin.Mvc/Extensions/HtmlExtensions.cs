using System.Globalization;
using System.Web;
using System.Web.Compilation;
using System.Web.Mvc;


namespace OrderTrackingAdmin.Mvc.Extensions
{
    public static class Language
    {
        public enum Name
        {
            en,
            fr,
            pl,
            bg,
            ru
        }
    }

    public static class HtmlExtensions
    {      
        #region Localization

       
        public static string Resource(this HtmlHelper htmlHelper, string expression, params object[] args)
        {
            var path = (string)htmlHelper.ViewData[SiteWebFormView.ViewPathKey];
            if (string.IsNullOrEmpty(path))
                path = "~/";

            var fields = GetResourceFields(expression, path);
            if (!string.IsNullOrEmpty(fields.ClassKey))
                return GetGlobalResource(fields, args);

            return GetLocalResource(path, fields, args);
        }

        static string GetLocalResource(string path, ResourceExpressionFields fields, object[] args)
        {
            return string.Format((string)HttpContext.GetLocalResourceObject(path, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
        }

        static string GetGlobalResource(ResourceExpressionFields fields, object[] args)
        {
            return string.Format((string)HttpContext.GetGlobalResourceObject(fields.ClassKey, fields.ResourceKey, CultureInfo.CurrentUICulture), args);
        }

        static ResourceExpressionFields GetResourceFields(string expression, string virtualPath)
        {
            var context = new ExpressionBuilderContext(virtualPath);
            var builder = new ResourceExpressionBuilder();
            return (ResourceExpressionFields)builder.ParseExpression(expression, typeof(string), context);
        }


    }
        #endregion
}
