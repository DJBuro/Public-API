using AndroAdmin.Services.Notifications;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AndroAdmin.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static HtmlString GetHttpOrHttps(this HtmlHelper html) 
        {
            var httpContext = html.ViewContext.HttpContext;

            return httpContext.Request.IsSecureConnection ? 
                new HtmlString("https://") : 
                new HtmlString("http://");
        }

        public static HtmlString ToJson<TViewModel>(this HtmlHelper<TViewModel> html, 
            object model,
            bool lowercaseName = true,
            bool indent = false)
        {
            var settings = new JsonSerializerSettings();
            if (lowercaseName)
            { 
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            var indentFormat = indent ? Formatting.Indented : Formatting.None;

            return new HtmlString(JsonConvert.SerializeObject(model, indentFormat, settings));
        }

        public static HtmlString ToJsonFromArray<TViewModel, TOriginalModel, TJsonModel>(this HtmlHelper<TViewModel> html,
           IEnumerable<TOriginalModel> model,
           Func<TOriginalModel, TJsonModel> convertModel,
           bool lowercaseName = true,
           bool indent = false)
        {
            List<TJsonModel> jsonList = new List<TJsonModel>();

            foreach (var item in model)
            {
                var tItem = convertModel(item);
                jsonList.Add(tItem);
            }

            var settings = new JsonSerializerSettings();
            if (lowercaseName) { 
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }
            var indentFormat = indent ? Formatting.Indented : Formatting.None;

            return new HtmlString(JsonConvert.SerializeObject(jsonList, indentFormat, settings));
        }
    }

    public static class HtmlHelperExtensionsForNotifications 
    {
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
    }
}