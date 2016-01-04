using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MyAndromeda.Services.Orders.Helpers
{
    public static class HtmlHelpers
    {
        public const string SuccessKey = "Emails.SuccessDistribution";
        public const string RejectedKey = "Emails.RejectedDistribution";
        public const string OrderManagmentKey = "Emails.OrderManagementDistribution";
        public const string RefundNotificationKey = "Emails.RefundNotifications";

        /// <summary>
        /// Success distribution list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static MvcHtmlString SuccessDistribution(this HtmlHelper html) 
        {
            var v = ConfigurationManager.AppSettings[SuccessKey];

            return string.IsNullOrWhiteSpace(v) ? new MvcHtmlString(string.Empty) : new MvcHtmlString(v);
        }

        /// <summary>
        /// Rejected distribution list..
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static MvcHtmlString RejectedDistribution(this HtmlHelper html)
        {
            var v = ConfigurationManager.AppSettings[RejectedKey];

            return string.IsNullOrWhiteSpace(v) ? new MvcHtmlString(string.Empty) : new MvcHtmlString(v);
        }

        /// <summary>
        /// Order management distribution list.
        /// </summary>
        /// <param name="html">The HTML.</param>
        /// <returns></returns>
        public static MvcHtmlString OrderManagementDistribution(this HtmlHelper html)
        {
            var v = ConfigurationManager.AppSettings[OrderManagmentKey];

            return string.IsNullOrWhiteSpace(v) ? new MvcHtmlString(string.Empty) : new MvcHtmlString(v);
        }

        public static MvcHtmlString RefundEmailDistribution(this HtmlHelper html) 
        {
            var v = ConfigurationManager.AppSettings[RefundNotificationKey];

            return string.IsNullOrWhiteSpace(v) ? new MvcHtmlString(string.Empty) : new MvcHtmlString(v);
        }
    }
}
