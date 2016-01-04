using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using OrderTracking.Dao;
using OrderTracking.Dao.Entity;

namespace OrderTrackingAdmin.Mvc
{
    public class SiteController : SiteBaseController
    {
        public enum StoreControllerViews
        {
            Index,
            Add,
            All,
            Drivers,
            GlobalLogs,
            Logs,
            Orders,
            Trackers,
            Result,
            Map
        }

        public enum TrackersControllerViews
        {
            Index,
            Add,
            All,
            Tracker,
            Setup,
            Map
        }
        public enum GlobalControllerViews
        {
            Index,
            Apn,
            AddApn,
            Sms,
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

            var valueProviderDictionary = new ValueProviderDictionary(this.ControllerContext);

            //bug: MVC, you need to remove routes before you can update... odd.                 
            foreach (var rdValues in this.ControllerContext.RouteData.Values)
            {
                valueProviderDictionary.Remove(rdValues.Key);
            }

            return TryUpdateModel(model, valueProviderDictionary);
        }
    }

    public class SiteBaseController : Controller
    {
        public IStoreDao StoreDao;
        public IAccountDao AccountDao;
        public String LanguageCode { get; set; }

        //Override the View
        protected ActionResult View(Enum viewName, Object viewData)
        {
            if (viewData is ISiteViewData)
            {
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
            data.LanguageCode = LanguageCode;
        }

        #region Localization


        protected override void Execute(RequestContext requestContext)
        {
            LanguageCode = Utilities.Cookie.GetLanguageCookie(requestContext.HttpContext.Request);

            var culture = CultureInfo.CreateSpecificCulture(LanguageCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            
            base.Execute(requestContext);
        }

        #endregion 
    }
}
