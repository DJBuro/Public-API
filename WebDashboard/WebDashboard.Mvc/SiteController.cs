using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using WebDashboard.Dao;
using WebDashboard.Dao.Entity;

namespace WebDashboard.Mvc
{
    public class SiteController : SiteBaseController
    {
        public enum AdminControllerViews
        {
            Index,
            Details,
            Dashboard,
            Indicator,
            Users,
            User,
            Permissions,
            Translation,
            Sites,
            Site,
            Region,
            Regions,
            RegionalSites,
            RegionalStore,
            Add,
            FlexData,
            ValueTypes, 
            Message, 
            Historical,
            Store,
            Logs
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
            var valueProviderDictionary = new FormValueProvider(this.ControllerContext);


            //bug: MVC, you need to remove routes before you can update... odd.                 
            //foreach (var rdValues in ControllerContext.RouteData.Values)
            //{
            //    valueProviderDictionary.Remove(rdValues.Key);
            //}

            return TryUpdateModel(model, valueProviderDictionary);
        }

    }


    /// <summary>
    /// Available Everywhere in site
    /// </summary>
    public class SiteBaseController : Controller
    {
        public String LanguageCode { get; set; }
        
        public ISiteDao SiteDao { get; set; }
        public IGroupExchangeRateDao GroupExchangeRateDao { get; set; }
        public IHeadOfficeDao HeadOfficeDao { get; set; }
        public IRegionDao RegionDao { get; set; }
        public IUserRegionDao UserRegionDao { get; set; }
 
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

        //protected override void Execute(RequestContext requestContext)
        //{
        //    LanguageCode = Utilities.Cookie.GetLanguageCookie(requestContext.HttpContext.Request);

        //    var culture = CultureInfo.CreateSpecificCulture(LanguageCode);
        //    Thread.CurrentThread.CurrentCulture = culture;
        //    Thread.CurrentThread.CurrentUICulture = culture;

        //    base.Execute(requestContext);
        //}

    }
}
