using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Dashboard.Dao;
using Dashboard.Dao.Domain;

namespace Dashboard.Web.Mvc
{
    public class SiteController : SiteBaseController
    {
        public enum HomeControllerViews
        {
            Index
        }

        public enum AccountControllerViews
        {
            Logon
        }

        public enum ScrollerControllerViews
        {
            Index,
            Ticket,
            OTD,
            OPD
            
        }

        public enum DashboardControllerViews
        {
            Index,
            Display,
            Site,
            Region
        }

        public enum DisplayControllerViews
        {
            Index,
            Site,
            HeadOffice
        }

        public enum HeadOfficeControllerViews
        {
            Index
        }

        public enum AdminControllerViews
        {
            Index,
            Details,
            Dashboard,
            Translation,
            Sites,
            Site,
            Region,
            RegionSites,
            AddSite,
            AddSiteRegion,
            EditSite
        }


        protected new bool UpdateModel<TModel>(TModel model) where TModel : Entity //As we only want to inherit from valid classes.
        {
            return TryUpdateModel(model);
        }

        /// <summary>
        /// If we are doing consecutive updates, then use TryUpdateModel
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        protected new bool TryUpdateModel<TModel>(TModel model) where TModel : Entity //As we only want to inherit from valid classes.
        {
            if (model == null)
            {
                throw new ArgumentNullException("model cannot be null");
            }

            //var valueProviderDictionary = new ValueProviderDictionary(this.ControllerContext);

            var valueProviderDictionary = new ValueProviderDictionary(this.ControllerContext);


            //bug: MVC, you need to remove routes before you can update... odd.                 
            foreach (var rdValues in this.ControllerContext.RouteData.Values)
            {
                valueProviderDictionary.Remove(rdValues.Key);
            }

            return TryUpdateModel(model, valueProviderDictionary);
        }

    }


    /// <summary>
    /// Available Everywhere in site
    /// </summary>
    public class SiteBaseController : Controller
    {
        public IUserDao UserDao { get; set; }

        private User _User;
        public User User
        {
            get
            {
                if (_User == null)
                {
                    _User = UserDao.FindById(Dashboard.Web.Mvc.Utilities.Cookie.GetAuthoriationCookieSiteId(this.Request));
                }
                return _User;
            }
        }


        public IHeadOfficeDao HeadOfficeDao { get; set; }

        public ISiteDao SiteDao { get; set; }
        public IRegionDao RegionDao { get; set; }
        public Dash Dash { get; set; }


        //Override the View
        protected ActionResult View(Enum viewName, Object viewData)
        {
            if (viewData is ISiteViewData)
            {
                //Dashboard.Web.Mvc.Utilities.Cookie.GetAuthoriationCookie(this.Request);
                PrepareViewData((ISiteViewData)viewData);
            }
            return View(viewName.ToString(), viewData);
        }

        protected ActionResult RedirectToAction(Enum viewName, Object viewData)
        {
            return RedirectToAction(viewName.ToString(), viewData);
        }



        protected virtual void PrepareViewData(ISiteViewData data)
        {

            //todo: test ! Make sure the user is added, needed in the view>
            //if(data.ViewData == null)
            //    data.ViewData = new ViewDataDictionary();

            //data.ViewData.Add("user",this.User);
           

        }

    }
}
