using System;
using System.Globalization;
using System.Threading;
using System.Web.Mvc;
using System.Web.Routing;
using AndroAdmin.Dao;
using AndroAdmin.Dao.Entity;


namespace AndroAdmin.Mvc
{
    public class SiteController : SiteBaseController
    {
        public IAndroUserDao AndroUserDao { get; set; }

        public enum HomeControllerViews
        {
            Index,
            Start
        }       
        
        public enum TranslationControllerViews
        {
            Index,
            Translate
        }

        public enum AdminControllerViews
        {
            AllUsers,
            AddUser,
            EditUser,
            Permissions
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

        public String LanguageCode { get; set; }

        //Override the View
        protected ActionResult View(Enum viewName, Object viewData)
        {
            if (viewData is ISiteViewData)
            {
                //AndroAdmin.Web.Mvc.Utilities.Cookie.GetAuthoriationCookie(this.Request);
                PrepareViewData((ISiteViewData)viewData);
                // ((ISiteViewData)viewData).AddParams(this.ViewData);
            }
            return View(viewName.ToString(), viewData);
        }

        protected ActionResult RedirectToAction(Enum viewName, Object viewData)
        {
            return RedirectToAction(viewName.ToString(), viewData);
        }



        protected virtual void PrepareViewData(ISiteViewData data)
        {
            //Make sure the user is added, needed in the view
            //data.ViewData.Model.AndroWebAdmin = this.AndroWebAdmin;

            data.LanguageCode = LanguageCode;
        }

        protected override void Execute(RequestContext requestContext)
        {
            LanguageCode = Utilities.Cookie.GetLanguageCookie(requestContext.HttpContext.Request);

            var culture = CultureInfo.CreateSpecificCulture(LanguageCode);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            base.Execute(requestContext);
        }
    }
}
