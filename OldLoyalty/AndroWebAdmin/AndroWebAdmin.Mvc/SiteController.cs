using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Loyalty.Dao;


namespace AndroWebAdmin.Mvc
{
    public class SiteController : SiteBaseController
    {
        public enum HomeControllerViews
        {
            Index
        }

        public enum GlobalControllerViews
        {
            Index,
            AddCountry,
            EditCountry,
            AddCurrency,
            EditCurrency,
            ViewLog,
            SiteLog
        }

        public enum CompanyControllerViews
        {
            Index,
            EditCompany,
            AddCompany,
            Sites,
            EditSite,
            AddSite,
            UserTitles,
            AddTitle,
            EditUserTitle
        }

        public enum LoyaltyAccountControllerViews
        {
            Index,
            Company,
            Status,
            Details
        }

        public enum LoyaltyCardControllerViews
        {
            Index,
            Details,
            Status,
            Company
        }

        public enum LoyaltyUserControllerViews
        {
            Index,
            Details
        }

        public enum SearchControllerViews
        {
            Index
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
        //Override the View
        protected ActionResult View(Enum viewName, Object viewData)
        {
            if (viewData is ISiteViewData)
            {
                //AndroWebAdmin.Web.Mvc.Utilities.Cookie.GetAuthoriationCookie(this.Request);
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
        }
    }
}
