using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Dashboard.Dao;
using Dashboard.Dao.Domain;
using Dashboard.Dao.NHibernate;
using Dashboard.Web.Mvc;
using Dashboard.Web.Mvc.Helpers;
using Dashboard.Web.WebSite.Models;
using System.Net;


namespace Dashboard.Web.WebSite.Controllers
{
    [HandleError]
    public class HomeController : SiteController
    {
        public IDashboardDataDao DashboardDataDao { get; set; }
        public IIndicatorDefinitionDao IndicatorDefinitionDao { get; set; }

        //PRG Pattern
        public ActionResult Index()
        {
            var data = new DashboardViewData.IndexViewData();

            //IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

            ////note: this is only for testing!!!!
            //if (this.HttpContext.Request.IsLocal)
            //{
            //    var siteId = "369";
            //    data.Site = SiteDao.FindById(Convert.ToInt32(siteId));
            //}
            //else
            //{
            //}


            //todo: test against blackberry
            //var browser = this.HttpContext.Request.Browser;

            //Mvc.Utilities.Cookie.GetAuthoriationCookie(Request);

           // var xyz = this.HttpContext.Request.UserHostAddress;

            data.Site = SiteDao.FindByIP(HttpContext.Request.UserHostAddress);

            //redirect failed ip
            if (data.Site == null)
                return RedirectToAction("Logon", "Account");

            //authorise the site
            Dashboard.Web.Mvc.Utilities.Cookie.SetAuthoriationCookie(data.Site, this.Response);

            return RedirectToAction("Site/" + data.Site.Id.Value, "Display");
           // return RedirectToAction("Logon", "Account");
        }


        //todo: use this?
        private double ExtractValue(string value)
        {

            try
            {
                Double pdblValue = 0;
                value = value.Replace("%", string.Empty);
                if (value.Contains(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.CurrencySymbol))
                {
                    pdblValue = Double.Parse(value, System.Globalization.NumberStyles.Any);
                    return pdblValue;
                }

                return double.Parse(value);
            }
            catch
            {
                return 0;
            }


        }

    }
}
