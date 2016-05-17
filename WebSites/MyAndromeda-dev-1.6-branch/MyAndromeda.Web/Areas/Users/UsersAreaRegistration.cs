using System.Web.Mvc;

namespace MyAndromeda.Web.Areas.Users
{
    public class UsersAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Users";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Users_default",
                "Users/Chain/{chainId}/{controller}/{action}/{id}",
                new 
                { 
                    action = "Index", 
                    id = UrlParameter.Optional
                }
            );

            context.MapRoute(
                "Users_list",
                "Users/{controller}/{action}/{id}",
                new
                {
                    action = "Index",
                    id = UrlParameter.Optional
                }
            );
        }
    }
}
