using System.Collections.Generic;
using System.Web.Mvc;
using WebDashboard.Dao;
using WebDashboard.Dao.Domain;
using WebDashboard.Mvc;
using WebDashboard.Mvc.Extensions;
using WebDashboard.Mvc.Filters;
using WebDashboard.Web.Models;

namespace WebDashboard.Admin.Controllers
{
    [HandleError]
    public class HeadOfficeController : SiteController
    {
        public IIndicatorDao IndicatorDao { get; set; }
        public ISiteTypeDao SiteTypeDao { get; set; }
        public IValueTypeDao ValueTypeDao { get; set; }
        public IDivisorTypeDao DivisorTypeDao { get; set; }
        public IIndicatorTypeDao IndicatorTypeDao { get; set; }
        public IDefinitionDao DefinitionDao { get; set; }
        public IUserDao UserDao { get; set; }
        public IPermissionDao PermissionDao { get; set; }


        public void Translate(string id)
        {
            //note this is an empty call only to fire off the SiteBaseController to change the language
        }

        [RequiresAuthorisation]
        public ActionResult Index()
        {
            var data = new WebDashboardViewData.PageViewData {HeadOffices = HeadOfficeDao.FindAll()};

            return View(AdminControllerViews.Index, data);
        }

        [RequiresAuthorisation]
        public ActionResult Add()
        {
            var data = new WebDashboardViewData.PageViewData();

            return View(AdminControllerViews.Add, data);
        }

        [RequiresAuthorisation]
        public ActionResult Details(int id)
        {
            var data = new WebDashboardViewData.PageViewData {HeadOffice = HeadOfficeDao.FindById(id)};

            return View(AdminControllerViews.Details, data);
        }

        [RequiresAuthorisation]
        public ActionResult Sites(int id)
        {


            var data = new WebDashboardViewData.PageViewData
                           {
                               HeadOffice = HeadOfficeDao.FindById(id),
                               SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name")
                           };

            data.RegionListItems = RegionDao.FindHeadOffice(data.HeadOffice, false).ToSelectList("Id", "Name"); //todo: Jquery this so the regions match the dd HeadOffice

            return View(AdminControllerViews.Sites, data);
        }

        [RequiresAuthorisation]
        public ActionResult Search(int? id)
        {
            if (!id.HasValue)
            {
                return Index();
            }

            var data = new WebDashboardViewData.PageViewData {Site = SiteDao.FindByRamesesId(id)};


            if (data.Site == null)
                return Index();

            data.RegionListItems = RegionDao.FindHeadOffice(data.Site.HeadOffice, false).ToSelectList("Id", "Name");//todo: Jquery this so the regions match the dd HeadOffice
            data.HeadOfficeListItems = HeadOfficeDao.FindAll().ToSelectList("Id", "Name");
            data.SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name");

            return View(AdminControllerViews.Site, data);
        }

        [RequiresAuthorisation]
        public ActionResult Site(int? id)
        {
            if (!id.HasValue)
            {
                return Index();
            }

            var data = new WebDashboardViewData.PageViewData {Site = SiteDao.FindById(id.Value)};


            if (data.Site == null)
                return Index();

            data.RegionListItems = RegionDao.FindHeadOffice(data.Site.HeadOffice, false).ToSelectList("Id", "Name");//todo: Jquery this so the regions match the dd HeadOffice
            data.HeadOfficeListItems = HeadOfficeDao.FindAll().ToSelectList("Id", "Name");
            data.SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name");

            return View(AdminControllerViews.Site, data);
        }

        [RequiresAuthorisation]
        public ActionResult Region(int id)
        {
            var data = new WebDashboardViewData.PageViewData {HeadOffice = HeadOfficeDao.FindById(id)};

            return View(AdminControllerViews.Region, data);
        }

        [RequiresAuthorisation]
        public ActionResult RegionalSites(int id)
        {
            var data = new WebDashboardViewData.PageViewData {Region = RegionDao.FindById(id)};

            data.Sites = SiteDao.FindAllRegion(data.Region);

            return View(AdminControllerViews.RegionalSites, data);
        }

        [RequiresAuthorisation]
        public ActionResult Dashboard(int id)
        {
            var data = new WebDashboardViewData.PageViewData {HeadOffice = HeadOfficeDao.FindById(id)};

            data.Indicators = IndicatorDao.GetIndictorsByHeadOffice(data.HeadOffice);
            data.ValueTypeListItems = ValueTypeDao.FindAll().ToSelectList("Id", "Name");

            return View(AdminControllerViews.Dashboard, data);
        }

        [RequiresAuthorisation]
        public ActionResult Indicator(int id, int trans)
        {
            var data = new WebDashboardViewData.PageViewData
                           {
                               HeadOffice = HeadOfficeDao.FindById(id),
                               Indicator = IndicatorDao.FindById(trans)
                           };

            data.ValueTypeListItems = ValueTypeDao.FindAll().ToSelectList("Id", "Name", data.Indicator.ValueType.Id.ToString());

            data.DivisorTypeListItems = DivisorTypeDao.FindAll().ToSelectList("Id", "Name", data.Indicator.DivisorType.Id.ToString());

            data.IndicatorTypeListItems = IndicatorTypeDao.FindAll().ToSelectList("Id", "Name", data.Indicator.IndicatorType.Id.ToString());

            data.DefinitionListItems = DefinitionDao.FindAll().ToSelectList("Id", "Name",data.Indicator.Definition.Id.ToString());

            return View(AdminControllerViews.Indicator, data);
        }
        
        [RequiresAuthorisation]
        public ActionResult Users(int id)
        {
            var data = new WebDashboardViewData.PageViewData {HeadOffice = HeadOfficeDao.FindById(id)};

            return View(AdminControllerViews.Users, data);
        }        
        
        [RequiresAuthorisation]
        public ActionResult User(int id)
        {
            var data = new WebDashboardViewData.PageViewData {User = UserDao.FindById(id)};
            
            data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);

            return View(AdminControllerViews.User, data);
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult CreateUser(User user)
        {
            var data = new WebDashboardViewData.PageViewData();

            if (UpdateModel(user))
            {
                UserDao.Create(user);
            }

            data.HeadOffice = HeadOfficeDao.FindById(user.HeadOffice.Id.Value);
            return (View(AdminControllerViews.Users, data));
        }  

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateUser(User user)
        {
            var data = new WebDashboardViewData.PageViewData { User = UserDao.FindById(user.Id.Value) };

            data.User.Active = user.Active;
            data.User.IsExecutiveDashboardUser = user.IsExecutiveDashboardUser;
            data.User.IsExecutiveDashboardGroupUser = user.IsExecutiveDashboardGroupUser;
            data.User.IsAdministrator = user.IsAdministrator;
            data.User.StoreUser = user.StoreUser;
            data.User.Password = user.Password;
            data.User.EmailAddress = user.EmailAddress;

            if (UpdateModel(data.User))
            {
                UserDao.Update(data.User);
            }

            data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);

            return View(AdminControllerViews.User, data);
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult RemoveUser(User user)
        {
            var data = new WebDashboardViewData.PageViewData { User = UserDao.FindById(user.Id.Value) };

            UserDao.Delete(data.User);
            
            data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);

            return View(AdminControllerViews.Users, data);
        }   
        
        [RequiresAuthorisation]
        public ActionResult Permissions(int id)
        {
            var data = new WebDashboardViewData.PageViewData {User = UserDao.FindById(id)};

            data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);

            data.Regions = RegionDao.FindHeadOffice(data.HeadOffice, false);

            return View(AdminControllerViews.Permissions, data);
        }

        [RequiresAuthorisation]
        public ActionResult AddPermission(int id, int trans)
        {
            var data = new WebDashboardViewData.PageViewData { User = UserDao.FindById(id) };

            data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);

            data.Regions = RegionDao.FindHeadOffice(data.HeadOffice, false);

            var site = SiteDao.FindByRamesesId(trans);
            
            var permission = new Permission(data.User, site);

            PermissionDao.Save(permission);

            return View(AdminControllerViews.Permissions, data);
        }

        [RequiresAuthorisation]
        public ActionResult RemovePermission(int id, int trans)
        {
            var user = UserDao.FindById(id);

            var permissions = PermissionDao.FindUserPermissions(user);

            foreach (var permission in permissions)
            {
                if (permission.Site.SiteId != trans) continue;
                PermissionDao.Delete(permission);
                break;
            }

            var data = new WebDashboardViewData.PageViewData { User = UserDao.FindById(id) };

            data.HeadOffice = HeadOfficeDao.FindById(data.User.HeadOffice.Id.Value);

            data.Regions = RegionDao.FindHeadOffice(data.HeadOffice, false);
            return View(AdminControllerViews.Permissions, data);
        }
         
        
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateIndicator(Indicator indicator)
        {
            var data = new WebDashboardViewData.PageViewData
                           {
                               Indicator = IndicatorDao.FindById(indicator.Id.Value)
                           };

            if (UpdateModel(data.Indicator))
            {
                IndicatorDao.Update(data.Indicator);

                data.HeadOffice = data.Indicator.HeadOffice;
                data.Indicators = IndicatorDao.GetIndictorsByHeadOffice(data.HeadOffice);
                data.ValueTypeListItems = ValueTypeDao.FindAll().ToSelectList("Id", "Name");
            }
            return (View(AdminControllerViews.Dashboard, data));
        }       
        
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateAdvancedIndicator(Indicator indicator)
        {
            var data = new WebDashboardViewData.PageViewData
                           {
                               Indicator = IndicatorDao.FindById(indicator.Id.Value)
                           };

            data.HeadOffice = data.Indicator.HeadOffice;

            UpdateModel(data.Indicator);

            IndicatorDao.Update(data.Indicator);

            data.ValueTypeListItems = ValueTypeDao.FindAll().ToSelectList("Id", "Name", data.Indicator.ValueType.Id.ToString());
            data.DivisorTypeListItems = DivisorTypeDao.FindAll().ToSelectList("Id", "Name", data.Indicator.DivisorType.Id.ToString());
            data.IndicatorTypeListItems = IndicatorTypeDao.FindAll().ToSelectList("Id", "Name", data.Indicator.IndicatorType.Id.ToString());
            data.DefinitionListItems = DefinitionDao.FindAll().ToSelectList("Id", "Name", data.Indicator.Definition.Id.ToString());

            return (View(AdminControllerViews.Indicator, data));
        }        
        
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult AddHeadOffice(HeadOffice headOffice)
        {
            var data = new WebDashboardViewData.PageViewData
                           {
                               Indicators = new List<Indicator>(),
                               HeadOffice = new HeadOffice {Indicators = new List<Indicator>()}
                           };

            data.HeadOffice = HeadOfficeDao.Create(headOffice);

            for (var i = 0; i < 7; i++)
            {
                var indicator = IndicatorDao.FindById(i+1);
                indicator.Id = null;
                indicator.HeadOffice = data.HeadOffice;
                data.HeadOffice.Indicators.Add(indicator);
                HeadOfficeDao.Update(data.HeadOffice);
            }
            
            

            return (View(AdminControllerViews.Details, data));
        }        
        
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateHeadOffice(HeadOffice headOffice)
        {
            var data = new WebDashboardViewData.PageViewData
                           {
                               HeadOffice = HeadOfficeDao.FindById(headOffice.Id.Value)
                           };

            if (UpdateModel(data.HeadOffice))
            {
                HeadOfficeDao.Update(headOffice);
            }

            data.HeadOffices = HeadOfficeDao.FindAll();

            return (View(AdminControllerViews.Index,data));
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult AddRegion(Region region)
        {
            var data = new WebDashboardViewData.PageViewData();

            if (UpdateModel(region))
            {
                RegionDao.Create(region);
            }

            data.HeadOffice = HeadOfficeDao.FindById(region.HeadOffice.Id.Value);

            return (View(AdminControllerViews.Region, data));
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateRegion(Region region)
        {
            var data = new WebDashboardViewData.PageViewData();

            if (UpdateModel(region))
            {
                RegionDao.Update(region);
            }

            data.HeadOffice = HeadOfficeDao.FindById(region.HeadOffice.Id.Value);

            return (View(AdminControllerViews.Region, data));
        }
       
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult CreateSite(Site site)
        {
            var data = new WebDashboardViewData.PageViewData();

            if (UpdateModel(site))
            {
                SiteDao.Create(site);
            }

            data.Site = SiteDao.FindById(site.Id.Value);

            data.Site.Region = RegionDao.FindById(data.Site.Region.Id.Value);

            data.HeadOffice = HeadOfficeDao.FindById(data.Site.HeadOffice.Id.Value);

            data.SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name");

            data.RegionListItems = RegionDao.FindHeadOffice(data.HeadOffice, false).ToSelectList("Id", "Name"); //todo: Jquery this so the regions match the dd HeadOffice

            return (View(AdminControllerViews.Sites, data));
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult UpdateSite(Site site)
        {
            var data = new WebDashboardViewData.PageViewData();

            if (UpdateModel(site))
            {
                SiteDao.Update(site);
            }

            data.Site = SiteDao.FindById(site.Id.Value);
            data.Site.HeadOffice = HeadOfficeDao.FindById(data.Site.HeadOffice.Id.Value);

            data.RegionListItems = RegionDao.FindHeadOffice(data.Site.HeadOffice, false).ToSelectList("Id", "Name");//todo: Jquery this so the regions match the dd HeadOffice
            data.HeadOfficeListItems = HeadOfficeDao.FindAll().ToSelectList("Id", "Name");
            data.SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name");

            return (View(AdminControllerViews.Site, data));
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult RemoveSite(Site site)
        {
            var data = new WebDashboardViewData.PageViewData();

            site = SiteDao.FindById(site.Id.Value);

            SiteDao.Delete(site);
    
            data.HeadOffice = HeadOfficeDao.FindById(site.HeadOffice.Id.Value);

            data.SiteTypeListItems = SiteTypeDao.FindAll().ToSelectList("Id", "Name");
            data.RegionListItems = RegionDao.FindHeadOffice(data.HeadOffice, false).ToSelectList("Id", "Name"); //todo: Jquery this so the regions match the dd HeadOffice

            return View(AdminControllerViews.Sites, data);
        }

        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult RemoveRegion(Region region)
        {
            var data = new WebDashboardViewData.PageViewData();

            region = RegionDao.FindById(region.Id.Value);
            RegionDao.Delete(region);

            data.HeadOffice = HeadOfficeDao.FindById(region.HeadOffice.Id.Value);
            
            return View(AdminControllerViews.Region, data);
        }        
        
        [RequiresAuthorisation]
        [HttpPost]
        public ActionResult RemoveHeadOffice(HeadOffice headOffice)
        {
            var data = new WebDashboardViewData.PageViewData
                           {
                               HeadOffice = HeadOfficeDao.FindById(headOffice.Id.Value)
                           };

            //Simple cascade delete
            HeadOfficeDao.Delete(data.HeadOffice);

            data.HeadOffices = HeadOfficeDao.FindAll();
            
            return View(AdminControllerViews.Index, data);
        }


    }
}
