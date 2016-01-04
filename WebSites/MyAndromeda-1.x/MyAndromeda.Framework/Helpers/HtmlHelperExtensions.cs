using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.RouteHelpers;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using System.Text.RegularExpressions;
using MyAndromeda.Framework.Authorization;
using System.Text;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Framework.Helpers
{
    public static class HtmlElementsHelperExtension
    {
        //public static MvcHtmlString ValueFor<TModel, TValue>(this HtmlHelper<TModel> html, Expression<Func<TModel, TValue>> predicate) 
        //{
        //    var bodyFunc = predicate.Compile();
        //    var value = bodyFunc(html.ViewData.Model);

        //    return new MvcHtmlString(value.ToString());
        //}

        public static MvcHtmlString ActionClientTemplate(this UrlHelper html, string action, string controller, object routeValues) 
        {
            var url = html.Action(action, controller, routeValues);
            var decode = System.Web.HttpUtility.UrlDecode(url);

            return new MvcHtmlString(decode);
        }

        public static MvcHtmlString RouteClientTemplate(this UrlHelper html, string routeName, object routeValues) 
        {
            var url = html.RouteUrl(routeName, routeValues);
            var decode = System.Web.HttpUtility.UrlDecode(url);

            return new MvcHtmlString(decode);
        }

        public static bool HasExceptionMessages(this HtmlHelper html) 
        {
            var o = html.ViewContext.Controller.TempData[Notifier.ExceptionKey];
            if (o == null) { return false; }

            var collection = o as ICollection<Exception>;

            return collection.Count > 0;
        }
        
        public static bool HasNotificationMessages(this HtmlHelper html)
        {
            var o = html.ViewContext.Controller.TempData[Notifier.MessageKey];
            if (o == null) { return false; ; }

            var collection = o as ICollection<string>;

            return collection.Count > 0;
        }

        public static bool HasNotificationErrorMessages(this HtmlHelper html) 
        {
            var o = html.ViewContext.Controller.TempData[Notifier.ErrorMessageKey];
            if (o == null) { return false; }

            var collection = o as ICollection<string>;

            return collection.Count > 0;
        }
        
        public static IEnumerable<string> NotificationMessages(this HtmlHelper html) 
        {
            var o = html.ViewContext.Controller.TempData[Notifier.MessageKey];
            if (o == null) { return Enumerable.Empty<string>(); }
            
            var collection = o as ICollection<string>;

            return collection;
        }

        public static IEnumerable<string> NotificationErrorMessage(this HtmlHelper html) 
        {
            var o = html.ViewContext.Controller.TempData[Notifier.ErrorMessageKey];
            if (o == null) { return Enumerable.Empty<string>(); }

            var collection = o as ICollection<string>;

            return collection;
        }

        public static IEnumerable<Exception> NotificationExceptionMessages(this HtmlHelper html)
        {
            var o = html.ViewContext.Controller.TempData[Notifier.ExceptionKey];
            if (o == null) { return Enumerable.Empty<Exception>(); }

            var collection = o as ICollection<Exception>;

            return collection;
        }

        public static string GetExternalSiteIdFromRoute(this HtmlHelper html) 
        {
            //var context = new HttpContextWrapper(HttpContext.Current);
            //RouteTable.Routes.GetRouteData(context);
            var routeData = html.ViewContext.RouteData;
            var routeDataValue = routeData.Values
                .Where(e=> e.Key.Equals("ExternalSiteId", StringComparison.InvariantCultureIgnoreCase))
                .Select(e=> e.Value)
                .FirstOrDefault();
 
            if (routeDataValue == null)
                return string.Empty;
            
            return routeDataValue.ToString(); 
        } 

        public static ControllerRoute ControllerRoute(this HtmlHelper html)
        {
            var routeData = html.ViewContext.RouteData;

            return new ControllerRoute(routeData);
        }

        /// <summary>
        /// Gets the translator.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static ITranslator GetTranslator(this HtmlHelper html) 
        {
            return DependencyResolver.Current.GetService<ITranslator>();
        }

        public static MvcHtmlString WriteIfNotNullOrEmpty<TModel>(this HtmlHelper<TModel> html, Expression<Func<TModel, string>> textDelegate) 
        {
            var model = html.ViewData.Model;

            var getValueDelegate = textDelegate.Compile();
            var value = getValueDelegate(model);

            return new MvcHtmlString(string.IsNullOrWhiteSpace(value) ? string.Empty : value + "<br />");
        }

        public static MvcHtmlString WriteIfNotNullOrEmpty(this HtmlHelper html, string value) 
        {
            return new MvcHtmlString(string.IsNullOrWhiteSpace(value) ? string.Empty : value);
        }

        public static IAuthorizer GetAuthorizer(this HtmlHelper html) 
        {
            return DependencyResolver.Current.GetService<IAuthorizer>();
        }

        public static IApplicationSettings GetApplicationSettings(this HtmlHelper html) 
        {
            return DependencyResolver.Current.GetService<IApplicationSettings>();
        }

        public static DateTime? ConvertUtcToLocal(this HtmlHelper html, DateTime dateTime) 
        {
            var dateServices = DependencyResolver.Current.GetService<IDateServices>();

            return dateServices.ConvertToLocalFromUtc(dateTime);
        }

        public static string CamelFriendly(this string camel)
        {
            if (String.IsNullOrWhiteSpace(camel))
                return string.Empty;

            var sb = new StringBuilder(camel);

            for (int i = camel.Length - 1; i > 0; i--)
            {
                var current = sb[i];
                if ('A' <= current && current <= 'Z')
                {
                    sb.Insert(i, ' ');
                }
            }

            return sb.ToString();
        }
    }
}
