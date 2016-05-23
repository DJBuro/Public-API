using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace MyAndromeda.Framework.RouteHelpers
{
    public class ControllerRoute
    {
        private RouteData routeData;

        public ControllerRoute(RouteData routeData) : this()
        {
            this.routeData = routeData;
        }

        public ControllerRoute() 
        {
            this.Init();
        }

        private void Init() 
        {
            if (routeData == null) 
            {
                var context = new HttpContextWrapper(HttpContext.Current);
                this.routeData = RouteTable.Routes.GetRouteData(context);
            }

            object area = null;
            object controller = GetValue("Controller");
            object action = GetValue("Action");
            
            routeData.DataTokens.TryGetValue("Area", out area);
            
            this.Area = area == null ? string.Empty : area.ToString();
            this.Controller = controller == null ? string.Empty : controller.ToString();
            this.Action = action == null ? string.Empty : action.ToString();

        }

        public string Area { get; set; }

        public string Controller { get; set; }

        public string Action { get; set; }

        public object GetValue(string key) 
        {
            return routeData.Values
                .Where(e => e.Key.Equals(key, StringComparison.InvariantCultureIgnoreCase))
                .Select(e => e.Value)
                .FirstOrDefault();
        }
    }
}
