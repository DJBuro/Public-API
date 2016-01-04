using System.Web;
using System.Web.Mvc;

namespace MyAndromeda.WebApiServices
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}