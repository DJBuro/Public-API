using MyAndromeda.Core;
using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace MyAndromeda.Framework.Contexts
{
    public interface ICurrentRequest : IDependency
    {
        /// <summary>
        /// Gets the available.
        /// </summary>
        /// <value>The available.</value>
        bool Available { get; }

        /// <summary>
        /// Gets the debug mode.
        /// </summary>
        /// <value>The debug mode.</value>
        bool DebugMode { get; }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <value>The route data.</value>
        RouteData RouteData { get; }

        /// <summary>
        /// Gets the HTTP request.
        /// </summary>
        /// <value>The request.</value>
        HttpRequest Request { get; }

        /// <summary>
        /// Gets the HTTP context.
        /// </summary>
        /// <value>The HTTP context.</value>
        HttpContext HttpContext { get; }

        /// <summary>
        /// Gets the route data.
        /// </summary>
        /// <param name="parameter">The parameter.</param>
        /// <returns></returns>
        object GetRouteData(string parameter);

        /// <summary>
        /// Gets the chain id.
        /// </summary>
        /// <value>The chain id.</value>
        int? ChainId { get; }

        /// <summary>
        /// Gets the external site id.
        /// </summary>
        /// <value>The external site id.</value>
        string ExternalSiteId { get; }
    }
}
