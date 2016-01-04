using AndroAdmin.Helpers;
using AndroAdmin.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataWarehouseDataAccess;
using AndroAdminDataAccess.Domain;
using AndroAdminDataAccess.DataAccess;
using CloudSync;
using AndroAdminDataAccess.EntityFramework;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using System.Globalization;
using DataWarehouseDataAccess.DataAccess;
using DataWarehouseDataAccess.Domain;

namespace AndroAdmin.Controllers
{
    [Authorize]
    [Security(Permissions = "ViewACSMetrics")]
    public class MetricsController : BaseController
    {
        public MetricsController()
        {
            ViewBag.SelectedMenu = MenuItemEnum.OnlineOrdering;
            ViewBag.SelectedWebOrderingMenu = WebOrderingMenuItemEnum.Metrics;
        }
        
        // GET: /Metrics/
        [Security(Permissions = "ViewACSMetrics")]
        public ActionResult Index()
        {
            try
            {
                // Get the list of orders
                OrderMetricsViewModel vm = new OrderMetricsViewModel();
                vm.OrderMetrics = new DataWarehouseDataAccess.Domain.OrderMetrics();
                vm.OrderMetrics.OrderList = new List<DataWarehouseDataAccess.Domain.OrderHeaderDAO>();
                vm.FromDate = DateTime.Now.Date.AddDays(-7);
                vm.ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 59);
                //GetReportViewModel(vm);
               ViewBag.ErrorCodes = new SelectList(this.OrderMetricsDAO.GetAllACSErrorCodes(), "ErrorCode", "ShortDescription");
               ViewBag.OrderStatus = new SelectList(this.OrderMetricsDAO.GetAllOrderStatus(), "Id", "Description");
                return View(vm);
            }
            catch (Exception ex)
            {
                ErrorHelper.LogError("MetricsController.Index", ex);
                return RedirectToAction("Index", "Error");
            }
        }

        [HttpPost]
        public ActionResult Index(OrderMetricsViewModel vm)
        {
            GetReportViewModel(vm);
            ViewBag.ErrorCodes = new SelectList(this.OrderMetricsDAO.GetAllACSErrorCodes(), "ErrorCode", "ShortDescription");
            ViewBag.OrderStatus = new SelectList(this.OrderMetricsDAO.GetAllOrderStatus(), "Id", "Description");
            return View(vm);
        }

        public ActionResult GridData([DataSourceRequest] DataSourceRequest request,
            String chainId,
            String appId,
            String siteId,
            DateTime fromDate,
            DateTime toDate)
        {
            OrderMetricsViewModel vm = new OrderMetricsViewModel();
            vm.FromDate = fromDate;
            vm.ToDate = toDate;
            if (vm.FromDate == null || vm.ToDate == null)
            {
                vm.FromDate = DateTime.Now.Date.AddDays(-7);
                vm.ToDate = DateTime.Now.Date + new TimeSpan(23, 59, 59);
            }
            vm.ChainId = string.IsNullOrEmpty(chainId) ? null : (int?)Convert.ToInt32(chainId);
            vm.ACSApplicationId = string.IsNullOrEmpty(appId) ? null : (int?)Convert.ToInt32(appId);
            vm.SiteId = string.IsNullOrEmpty(siteId) ? null : (Guid?)new Guid(siteId);
            GetReportViewModel(vm);
            return Json(vm.OrderMetrics.OrderList.ToDataSourceResult(request));
        }

        //private DateTime? getDate(string dateString)
        //{
        //    if (string.IsNullOrEmpty(dateString))
        //        return null;
        //    // to be replaced with kendo - not working as expected; this is work-around - converts to dd/MM/yyyy (UK culture)
        //    dateString = dateString.Replace(" 12:00:00 AM", string.Empty);
        //    string[] dateParts = dateString.Split('/');
        //    string finalDate = string.Empty;
        //    foreach (string datePart in dateParts)
        //        finalDate += datePart.Length == 1 ? ("0" + datePart) + "/" : datePart + "/";
        //    DateTime result;
        //    DateTime.TryParseExact(finalDate.TrimEnd('/'), "MM/dd/yyyy", new CultureInfo("en-GB"), DateTimeStyles.None, out result);
        //    return result;
        //}

        private void GetReportViewModel(OrderMetricsViewModel vm)
        {
            // only chain specified in parameters
            List<string> siteList = new List<string>();
            if (vm.ChainId != null && (vm.ACSApplicationId == null && vm.SiteId == null))
            {
                IList<ACSApplicationEF> acsApplicationsEF = this.ACSApplicationEFDAO.GetAll();
                IList<ACSApplicationSite> acsApplicationsForChain = acsApplicationsEF.Select(a => a.ACSApplicationSites.Where(s => s.Store.ChainId.Equals(vm.ChainId))).SelectMany(s => s).ToList();
                IList<AndroAdminDataAccess.EntityFramework.Store> storeList = acsApplicationsForChain.Select(s => s.Store).Distinct().ToList();
                if (storeList.Count > 0)
                {
                    siteList = storeList.Select(x => x.ExternalId).ToList();
                }
            }
            DataWarehouseDataAccess.Domain.OrderMetrics orderMetrics = null;
            if (vm.SiteId != null)
            {
                siteList.Add(vm.SiteId.ToString());
            }
            this.OrderMetricsDAO.GetOrderMetrics(vm.FromDate, vm.ToDate + new TimeSpan(23, 59, 59),
                vm.ACSApplicationId, siteList, out orderMetrics);
            vm.OrderMetrics = orderMetrics;
        }

        public JsonResult GetChains()
        {
            IList<AndroAdminDataAccess.Domain.Chain> chains = this.ChainDAO.GetAll();
            return Json(chains, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetACSApplications(int? chain)
        {
            if (chain == null)
            {
                IList<AndroAdminDataAccess.Domain.ACSApplication> acsApplications = this.ACSApplicationDAO.GetAll();
                return Json(acsApplications, JsonRequestBehavior.AllowGet);
            }
            IList<ACSApplicationEF> acsApplicationsEF = this.ACSApplicationEFDAO.GetAll();
            IList<ACSApplicationSite> acsApplicationsForChain = acsApplicationsEF.Select(a => a.ACSApplicationSites.Where(s => s.Store.ChainId.Equals(chain))).SelectMany(s => s).ToList();
            IList<AndroAdminDataAccess.EntityFramework.ACSApplication> appList = acsApplicationsForChain.Select(x => x.ACSApplication).Distinct().ToList();
            List<AndroAdminDataAccess.Domain.ACSApplication> retList = new List<AndroAdminDataAccess.Domain.ACSApplication>();
            foreach (AndroAdminDataAccess.EntityFramework.ACSApplication app in appList)
                retList.Add(app.ToDomainObject());
            return Json(retList, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStores(int? chain, int? application)
        {
            if (chain == null && application == null)
            {
                IList<AndroAdminDataAccess.Domain.Store> stores = this.StoreDAO.GetAll();
                return Json(stores, JsonRequestBehavior.AllowGet);
            }
            IList<ACSApplicationEF> acsApplicationsEF = this.ACSApplicationEFDAO.GetAll();
            IList<ACSApplicationSite> acsApplicationsForChain = acsApplicationsEF.Select(a => a.ACSApplicationSites.Where(s => s.Store.ChainId.Equals(chain))).SelectMany(s => s).ToList();
            IList<AndroAdminDataAccess.EntityFramework.ACSApplication> appList = acsApplicationsForChain.Select(x => x.ACSApplication).Distinct().ToList();

            List<AndroAdminDataAccess.Domain.Store> retList = new List<AndroAdminDataAccess.Domain.Store>();
            if (application == null)
            {
                IList<AndroAdminDataAccess.EntityFramework.Store> storeList = acsApplicationsForChain.Select(s => s.Store).Distinct().ToList();
                foreach (AndroAdminDataAccess.EntityFramework.Store store in storeList)
                    retList.Add(store.ToDomainObject());
            }
            else
            {
                AndroAdminDataAccess.EntityFramework.ACSApplication selectedAPP = appList.Where(x => x.Id.Equals(application)).SingleOrDefault();
                IList<AndroAdminDataAccess.EntityFramework.Store> appStoreList = selectedAPP.ACSApplicationSites.Where(s => s.Store.ChainId == chain).Select(s => s.Store).Distinct().ToList();
                foreach (AndroAdminDataAccess.EntityFramework.Store store in appStoreList)
                    retList.Add(store.ToDomainObject());
            }
            return Json(retList, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(string id)
        {
            OrderMetricsViewModel vm = new OrderMetricsViewModel();
            vm.OrderMetrics = this.OrderMetricsDAO.GetOrderMetricsByACSOrder(new Guid(id));
            if (vm.OrderMetrics != null && vm.OrderMetrics.OrderList.Count == 1)
                return View("OrderDetails", vm.OrderMetrics.OrderList.FirstOrDefault());
            else
                return View("OrderDetails");
        } 
    }
}
