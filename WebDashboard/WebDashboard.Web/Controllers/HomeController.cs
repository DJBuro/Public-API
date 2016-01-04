using System.Web.Mvc;
using WebDashboard.Dao;
using WebDashboard.Dao.Domain;
using WebDashboard.Mvc;
using WebDashboard.Mvc.Filters;
using WebDashboard.Mvc.Helpers;
using WebDashboard.Web.Models;
using WebDashboard.Mvc.Extensions;
using System;
using System.IO;

namespace WebDashboard.Web.Controllers
{
    [HandleError]
    public class HomeController : SiteController
    {
        public IIndicatorDao IndicatorDao { get; set; }
        public IUserDao UserDao { get; set; }
        public IHistoricalDataDao HistoricalDataDao { get; set; }
        public IPermissionDao PermissionDao { get; set; }
        public ISiteTypeDao SiteTypeDao { get; set; }

        public ActionResult Index()
        {
            var data = new WebDashboardViewData.PageViewData();

            if(Request.IsAuthenticated)
            {
                User user = GetUser();
                if (user.IsAdministrator)
                {
                    return RedirectToAction("Sites", "Home");
                }
            }

            return View(AdminControllerViews.Index, data);
        }

        [RequiresAuthorisation]
        public ActionResult Message()
        {
            var data = new WebDashboardViewData.PageViewData {User = GetUser()};

            return View(AdminControllerViews.Message, data);
        }

        [RequiresAuthorisation]
        public ActionResult Sites()
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, false);
            data.UserRegions = UserRegionDao.FindByUserId(data.User.Id.Value);

            return View(AdminControllerViews.Sites, data);
        }

        [RequiresAuthorisation]
        public ActionResult Site(int id)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.Site = SiteDao.FindById(id);

                if (data.User.HeadOffice.Id != data.Site.HeadOffice.Id)
                {
                    //if they are messing with the URL to admin other HO users, we delete them.
                    SpoofedUser(data.User);
                    return View(AdminControllerViews.Index, data);
                }

                data.RegionListItems = RegionDao.FindHeadOffice(data.Site.HeadOffice, false).ToSelectList("Id", "Name");
                data.SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name");

                return View(AdminControllerViews.Site, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateSite(Site site)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                if (UpdateModel(site))
                {
                    SiteDao.Update(site);
                }

                data.Site = SiteDao.FindById(site.Id.Value);
                data.Site.HeadOffice = HeadOfficeDao.FindById(data.Site.HeadOffice.Id.Value);

                data.RegionListItems = RegionDao.FindHeadOffice(data.Site.HeadOffice, false).ToSelectList("Id", "Name");
                data.SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name");

                return this.Site(site.Id.Value);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult Regions()
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);
                
                return View(AdminControllerViews.Regions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult Regions(Region region)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                if (UpdateModel(region))
                {
                    region.HeadOffice = data.User.HeadOffice;
                    RegionDao.Create(region);

                    data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);
                }

                return View(AdminControllerViews.Regions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult RemoveRegion(Region region)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                region = RegionDao.FindById(region.Id.Value);
                if(region.RegionalSites.Count == 0)
                {
                    RegionDao.Delete(region);
                }

                data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);

                //bug: if we delete more than 2 in a row... falls over.
                return View(AdminControllerViews.Regions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateRegion(Region region)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                if (UpdateModel(region))
                {
                    RegionDao.Update(region);
                }

                return RedirectToAction("Regions");
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult RegionalSites(int id)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.Region = RegionDao.FindById(id);

                data.Sites = SiteDao.FindAllRegion(data.Region);

                return View(AdminControllerViews.RegionalSites, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult Users()
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.Users = UserDao.FindAllByHeadOffice(data.User.HeadOffice);

                return View(AdminControllerViews.Users, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }        
        
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                if(user.EmailAddress == null | user.Password == null)
                {
                    data.ErrorMessage = "Check Email Address/Password";
                }

                if(UpdateModel(user) && data.ErrorMessage==null)
                {
                    var exists = UserDao.CheckEmailExists(user.EmailAddress);

                    if (exists == null)
                    {
                        user.HeadOffice = data.User.HeadOffice;
                        UserDao.Create(user);
                    }
                    else
                    {
                        data.ErrorMessage = "That email address already exists";
                    }
                }

                data.Users = UserDao.FindAllByHeadOffice(data.User.HeadOffice);

                return View(AdminControllerViews.Users, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult User(int id)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.EditedUser = UserDao.FindById(id);

                if (data.User.HeadOffice.Id != data.EditedUser.HeadOffice.Id)
                {
                    //if they are messing with the URL to admin other HO users, we delete them.
                    SpoofedUser(data.User);
                    return View(AdminControllerViews.Index, data);
                }

                data.Regions = RegionDao.FindHeadOffice(data.EditedUser.HeadOffice, false);
                data.UserRegions = UserRegionDao.FindByUserId(data.EditedUser.Id.Value);
                
                return View(AdminControllerViews.User, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateUser(User user)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                if (UpdateModel(user) && data.ErrorMessage == null)
                {
                    var exists = UserDao.CheckEmailExists(user.EmailAddress);

                    if (exists.Id == user.Id)
                    {
                        user.HeadOffice = data.User.HeadOffice;
                        
                        UserDao.Update(user);
                    }
                    else
                    {
                        data.ErrorMessage = "That email address already exists";
                    }
                }

                data.EditedUser = user;

                return View(AdminControllerViews.User, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult RemoveUser(User user)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };
            
            if (data.User.IsAdministrator)
            {
                var delme = UserDao.FindById(user.Id.Value);

                UserDao.Delete(delme);
     
                data.Users = UserDao.FindAllByHeadOffice(data.User.HeadOffice);

                return View(AdminControllerViews.Users, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult Permissions(int id)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.EditedUser = UserDao.FindById(id);

                if (data.User.HeadOffice.Id != data.EditedUser.HeadOffice.Id)
                {
                    //if they are messing with the URL to admin other HO users, we delete them.
                    SpoofedUser(data.User);
                    return View(AdminControllerViews.Index, data);
                }

                data.Regions = RegionDao.FindHeadOffice(data.EditedUser.HeadOffice, false);
                data.UserRegions = UserRegionDao.FindByUserId(data.EditedUser.Id.Value);

                return View(AdminControllerViews.Permissions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult AddPermission(int id, int trans)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.EditedUser = UserDao.FindById(id);
                data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, false);

                var site = SiteDao.FindByRamesesId(trans);

                var permission = new Permission(data.EditedUser, site);

                PermissionDao.Save(permission);

                return View(AdminControllerViews.Permissions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult RemovePermission(int id, int trans)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.EditedUser = UserDao.FindById(id);

                var site = SiteDao.FindByRamesesId(trans);
                
                // Find the permission we need to delete
                Permission permissionToDelete = null;
                
                foreach (Permission permission in data.EditedUser.UserPermissions)
                {
                    if (permission.Site.Id == site.Id)
                    {
                        permissionToDelete = permission;
                        break;
                    }
                }

                // Delete the permission
                PermissionDao.Delete(permissionToDelete);

                data.EditedUser = UserDao.FindById(id);
                data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, false);

                return View(AdminControllerViews.Permissions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult EnableUserRegion(int id, int trans)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                data.EditedUser = UserDao.FindById(id);

                UserRegion userRegion = new UserRegion();
                userRegion.RegionId = trans;
                userRegion.UserId = id;

                UserRegionDao.Save(userRegion);

                data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, false);
                data.UserRegions = UserRegionDao.FindByUserId(id);

                return View(AdminControllerViews.Permissions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult DisableUserRegion(int id, int trans)
        {
            var data = new WebDashboardViewData.PageViewData { User = GetUser() };

            if (data.User.IsAdministrator)
            {
                var userRegions = UserRegionDao.FindByUserId(id);
                UserRegion deleteUserRegion = null;
                
                foreach (UserRegion userRegion in userRegions)
                {
                    if (userRegion.RegionId == trans)
                    {
                        deleteUserRegion = userRegion;
                        break;
                    }
                }

                if (deleteUserRegion != null)
                {
                    UserRegionDao.Delete(deleteUserRegion);
                }

                data.EditedUser = UserDao.FindById(id);
                data.Regions = RegionDao.FindHeadOffice(data.User.HeadOffice, false);
                data.UserRegions = UserRegionDao.FindByUserId(id);
                
                return View(AdminControllerViews.Permissions, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult Dashboard()
        {
            var data = new WebDashboardViewData.PageViewData {User = GetUser()};
            
            if (data.User.IsAdministrator)
            {
                data.HeadOffice = data.User.HeadOffice;
                data.Indicators = IndicatorDao.GetIndictorsByHeadOffice(data.User.HeadOffice);

                return View(AdminControllerViews.Dashboard, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult Dashboard(Indicator indicator)
        {
            var data = new WebDashboardViewData.PageViewData
                           {
                               User = GetUser(),
                               Indicator = IndicatorDao.FindById(indicator.Id.Value)
                           };

            if (data.User.IsAdministrator)
            {
                if (UpdateModel(data.Indicator))
                {
                    IndicatorDao.Update(data.Indicator);
                    data.Indicators = IndicatorDao.GetIndictorsByHeadOffice(data.User.HeadOffice);
                }

                data.HeadOffice = data.User.HeadOffice;

                return (View(AdminControllerViews.Dashboard, data));
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult SaveMessage(HeadOffice headOffice)
        {
            var data = new WebDashboardViewData.PageViewData {User = GetUser()};

            if (data.User.IsAdministrator)
            {
                var ho = HeadOfficeDao.FindById(headOffice.Id.Value);

                ho.Message = headOffice.Message;

                HeadOfficeDao.Save(ho);

                data.Indicators = IndicatorDao.GetIndictorsByHeadOffice(data.User.HeadOffice);

                return View(AdminControllerViews.Dashboard, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        [RequiresAuthorisation]
        public ActionResult Historical(int? id)
        {
            var data = new WebDashboardViewData.PageViewData {User = GetUser()};

            if (data.User.IsAdministrator)
            {
                var allSites = HistoricalDataDao.FindByHeadOffice(data.User.HeadOffice);

                data.Indicators = IndicatorDao.GetIndictorsByHeadOffice(data.User.HeadOffice);
                if(!id.HasValue)
                {
                    id = data.Indicators[0].Id;
                }
                data.Indicator = IndicatorDao.FindById(id.Value);

                if (data.Indicator.HeadOffice.Id != data.User.HeadOffice.Id.Value)
                {
                    //if they are messing with the URL to admin other HO users, we delete them.
                    SpoofedUser(data.User);
                    return View(AdminControllerViews.Index, data);
                }

                data.SiteRanks = ColumnData.RankSites(allSites, data.Indicator);

                return View(AdminControllerViews.Historical, data);
            }
            else
            {
                return RedirectToAction("Sites", "Home");
            }
        }

        //todo: expand if there isn't a user
        private User GetUser()
        {
            var userId = Mvc.Utilities.Cookie.GetAuthoriationCookieSiteId(HttpContext.Request);
            var user = UserDao.FindById(userId);
            return user;
        }

        //todo: destroy auth cookie/redirect to a view??
        private void SpoofedUser(User user)
        {
            //todo: log this!
            #if !DEBUG
                UserDao.Delete(user); 
            #endif
        }

        /// <summary>
        /// If they are not running the site through the app.
        /// </summary>
        public void IPLookup()
        {
            var userHostAddress = HttpContext.Request.UserHostAddress;

            var site = SiteDao.FindByIp(userHostAddress);

            if (site != null)
            {
                Response.Redirect("http://dashboard.androtechnology.co.uk/Flex2/#" + Obfuscator.encryptString(site.Key.ToString()), true);
            }
        }

        [HttpGet]
        [OutputCache(Duration = 30, VaryByParam = "id")]
        public JsonResult FlexData(string id)
        {
            try
            {
                FileLogger.LogEvent("FlexData called.  Id=" + id == null ? "null" : id);
            
                string licenseKeyString = Obfuscator.decryptString(id);
                int licenseKey = Int32.Parse(licenseKeyString);

                FileLogger.LogEvent("Decrypted string=" + licenseKeyString);
                Site site = SiteDao.FindBySiteKey(licenseKey);

                if (site == null)
                {
                    return null;
                }

                //todo: make sure it is a site - otherwise display as office
                //if (site.SiteType.Id != 1)

                var allSites = SiteDao.FindAllActiveSitesByHeadOffice(site.HeadOffice);

                var indicators = IndicatorDao.GetIndictorsByHeadOffice(site.HeadOffice);

                var dashboard = DashboardData.Get(site, indicators, allSites);

                dashboard.HeadOfficeMessage = site.HeadOffice.Message;

                /*This is needed for IE to pick up changes*/
                Response.AddHeader("Content-type", "text/json");
                Response.AddHeader("Cache-Control","no-cache, must-revalidate");
                Response.AddHeader("Expires", "0");

                var jsonResult = Json(dashboard,JsonRequestBehavior.AllowGet);

                return jsonResult;
            }
            catch (Exception exception)
            {
                FileLogger.LogEvent("Exception: " + exception.Message);
                return null;
            }
        }
    }
}
