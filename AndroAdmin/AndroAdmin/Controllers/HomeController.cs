using System.Web.Mvc;
using AndroAdmin.Dao;
using AndroAdmin.Models;
using AndroAdmin.Mvc;
using AndroAdmin.Mvc.Filters;

namespace AndroAdmin.Controllers
{
    [HandleError]
    public class HomeController : SiteController
    {
        
       
        public ActionResult Index()
        {
            var data = new AndroAdminViewData.IndexViewData();

            //Spring.Scheduling.Quartz.JobMethodInvocationFailedException

            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            if(userId != -1)
            {
                data.AndroUser = AndroUserDao.FindById(userId);

                return (View(HomeControllerViews.Start, data));
            }

            return (View(HomeControllerViews.Index, data));
        }


        [HttpPost]
        public ActionResult Login(string emailAddress, string password)
        {
            var data = new AndroAdminViewData.IndexViewData();

            data.AndroUser = AndroUserDao.FindByEmailAddressPassword(emailAddress, password);

            if(data.AndroUser !=null && data.AndroUser.Active)
            {
                Mvc.Utilities.Cookie.SetAuthoriationCookie(data.AndroUser, Response);

                return (View(HomeControllerViews.Start, data));
            }

            return (View(HomeControllerViews.Index, data));
        }

        [RequiresAuthorisation]
        public ActionResult Login()
        {
            var data = new AndroAdminViewData.IndexViewData();
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroUser = AndroUserDao.FindById(userId);

            return (View(HomeControllerViews.Start, data));

        }

        [RequiresAuthorisation]
        public ActionResult Start()
        {
            var data = new AndroAdminViewData.IndexViewData();

            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieUserId(Request);

            data.AndroUser = AndroUserDao.FindById(userId);

            return (View(HomeControllerViews.Start, data));
        }
    }
}
