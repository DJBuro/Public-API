using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace MyAndromeda.Framework.Mvc
{
    public static class KeepAlive
    {
        public static bool Poke() 
        {
            var url = VirtualPathUtility.ToAbsolute("~/"); //System.Web.Hosting.HostingEnvironment.MapPath("~/");
            var request = WebRequest.Create(url) as HttpWebRequest;

            if (request != null)
            {
                using (request.GetResponse() as HttpWebResponse) { }
                return true;
            }

            return false;
        }
    }
}
