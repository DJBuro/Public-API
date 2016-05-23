using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MyAndromeda.Framework.Contexts
{
    public class CurrentRequest : ICurrentRequest 
    {
        public CurrentRequest(RouteData routeData) 
        {
            this.RouteData = routeData;
            this.HttpContext = HttpContext.Current;
        }

        public bool Available { get { return this.HttpContext == null ? false : true; } }

        public bool DebugMode
        {
            get
            {
                if (this.Request.IsLocal) { return true; }
                if (this.GetRouteData("DebugMode") != null) { return true; }

                return false;
            }
        }

        public RouteData RouteData { get; private set; }

        public HttpContext HttpContext { get; private set; }

        public HttpRequest Request
        {
            get
            {
                if (this.HttpContext == null) 
                {
                    return null;
                }

                return this.HttpContext.Request;
            }
        }

        public object GetRouteData(string parameter)
        {
            var routeDataValue = this.RouteData.Values
                .Where(e => e.Key.Equals(parameter, StringComparison.InvariantCultureIgnoreCase))
                .Select(e => e.Value)
                .FirstOrDefault();

            if (routeDataValue != null) 
            {
                return routeDataValue;
            }

            //check if a key is available 
            var queryStringKey = this.Request.QueryString
                .AllKeys
                .Where(e => e.Equals(parameter, StringComparison.CurrentCultureIgnoreCase))
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(queryStringKey)) { return string.Empty; }

            //return the value from the key
            var queryStringValue = this.Request.QueryString[queryStringKey];

            return queryStringValue;
        }

        public int? ChainId
        {
            get
            {
                var value = this.GetRouteData("ChainId");
                if (value == null) return null;

                return int.Parse(value.ToString());
            }
        }

        public string ExternalSiteId
        {
            get
            {
                var value = this.GetRouteData("ExternalSiteId");
                if (value == null) return null;

                return value.ToString();
            }
        }
    }
}