using MyAndromeda.Data.DataAccess.WebOrdering;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Services.Vouchers.Services;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Web.Areas.Acs;
using MyAndromeda.Web.Areas.Voucher.Models;

namespace MyAndromeda.Web.Areas.Voucher.Controllers
{
    public class SiteVoucherCodesController : Controller
    {
        /* Utility services */
        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator;

        /* Context variables */
        private readonly ISiteVoucherService siteVoucherService;
        private readonly IWebOrderingWebSiteDataService websiteDataService;

        private readonly ICurrentSite currentSite;

        public SiteVoucherCodesController(IAuthorizer authorizer,
            INotifier notifier,
            ITranslator translator,
            ISiteVoucherService siteVoucherService,
            IWebOrderingWebSiteDataService websiteDataService,
            ICurrentSite site)
        {
            this.currentSite = site;
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.translator = translator;
            this.siteVoucherService = siteVoucherService;
            this.websiteDataService = websiteDataService;
        }

        //
        // GET: /Voucher/SiteVoucherCodes/

        public ActionResult Index()
        {
            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.ViewVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to view voucher codes"));
                return new HttpUnauthorizedResult();
            }

            return View();
        }

        public ActionResult List([DataSourceRequest] DataSourceRequest request)
        {
            if (!authorizer.Authorize(UserPermissions.ViewVoucherCodes)) 
            {
                return new HttpUnauthorizedResult();
            }

            var websites = this.websiteDataService.List()
                .Where(e => e.ACSApplication.ACSApplicationSites.Any(store => store.SiteId == this.currentSite.SiteId))
                .Select(e => new { e.LiveDomainName, e.PreviewDomainName })
                .ToArray() //load the data
                .Select(e => new VoucherWebSite(){ LiveWebsite = e.LiveDomainName, PreviewWebiste = e.PreviewDomainName })
                .ToArray(); //to something more useful

            var data = this.siteVoucherService.List();

            var result = data.ToDataSourceResult(request, e => e.ToViewModel(this.currentSite.Store.ExternalSiteName, websites));

            return Json(result, JsonRequestBehavior.AllowGet);

        }

        public ActionResult Enable(Guid id) 
        {
            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.EnableOrDisableVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to enable voucher codes"));
                return new HttpUnauthorizedResult();
            }

            var voucher = this.siteVoucherService.Get(id);

            if (voucher == null) 
            {
                this.notifier.Equals(translator.T("This voucher does not exist"));

                return RedirectToAction("Index");
            }

            voucher.Active = true;
            this.siteVoucherService.Update(voucher);

            return RedirectToAction("Index");
        }

        public ActionResult Disable(Guid id) 
        {
            var voucher = this.siteVoucherService.Get(id);
            if (voucher == null)
            {
                this.notifier.Equals(translator.T("This voucher does not exist"));

                return RedirectToAction("Index");
            }

            voucher.Active = false;
            this.siteVoucherService.Update(voucher);

            return RedirectToAction("Index");
        }

        /// <summary>
        /// Used to serach voucher codes
        /// </summary>
        /// <param name="active">voucher code removed or not</param>
        /// <param name="searchText">voucher code and description search</param>
        /// <returns>json result of voucher codes list</returns>
        //public JsonResult SearchVoucherCodes(string active, string searchText)
        //{
        //    var data = this.siteVoucherService.List().ToList<MyAndromeda.Data.DataWarehouse.Models.Voucher>();

        //    if (data != null && data.Count > 0 && (!string.IsNullOrEmpty(active)))
        //    {
        //        var results = data.Where(d => d.Removed != Convert.ToBoolean(Convert.ToInt32(active)));
        //        if (!string.IsNullOrEmpty(searchText))
        //        {
        //            if (searchText.Trim() != string.Empty)
        //            {
        //                results = results.Where(r => (r.VoucherCode.Contains(searchText) || r.Description.Contains(searchText))).ToList<MyAndromeda.Data.DataWarehouse.Models.Voucher>();
        //            }
        //        }
        //        var bindList = new List<VoucherViewModel>();
        //        if (data != null)
        //            bindList = convertToViewModel(data);

        //        return Json(bindList.ToList(), JsonRequestBehavior.AllowGet);
        //    }

        //    return null;
        //}

        //
        // GET: /Voucher/SiteVoucherCodes/Create

        public ActionResult Create()
        {
            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.EnableOrDisableVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to create voucher codes"));
                return new HttpUnauthorizedResult();
            }

            var viewModel = new VoucherViewModel();

            //defaults
            viewModel.IsActive = true;
            viewModel.SelectedOccasions = new List<string>() { "Delivery", "Collection" };
            viewModel.SelectedAvailableOnDays = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
            
            return this.View("Edit",viewModel);
        }

        //
        // POST: /Voucher/SiteVoucherCodes/Create

        [HttpPost]
        [ActionName("Create")]
        public ActionResult CreatePost(VoucherViewModel viewModel)
        {
            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.EnableOrDisableVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to create voucher codes"));
                return new HttpUnauthorizedResult();
            }

            try
            {
                var voucherInDB = this.siteVoucherService.GetByExpression(e => e.VoucherCode == viewModel.VoucherCode);
                if (voucherInDB != null)
                {
                    this.notifier.Error(translator.T("Voucher Code already exists. choose another name."));
                    return View("Edit", viewModel);
                }

                if (viewModel.StartDateTime > viewModel.EndDataTime)
                {
                    this.notifier.Error(translator.T("Start date time should be less than End date time"));
                    return View("Edit", viewModel);
                }

                var newVoucher = this.siteVoucherService.New();

                newVoucher.UpdateFromViewModel(viewModel);

                this.siteVoucherService.Create(newVoucher);
                this.notifier.Notify(this.translator.T("The voucher code is created"));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.notifier.Error(translator.T(ex.Message));
                return View("Edit", viewModel);
            }
        }

        private bool AreValidDates(VoucherViewModel viewModel)
        {
            if (viewModel.StartDateTime == null || viewModel.EndDataTime == null)
            {
                this.notifier.Error(translator.T("Please select start and end voucher availability"));
                return false;
            }
            else if (viewModel.StartTimeOfDayAvailable == null || viewModel.EndTimeOfDayAvailable == null)
            {
                this.notifier.Error(translator.T("Please select start and end voucher available"));
                return false;
            }
            else if (viewModel.StartDateTime.GetValueOrDefault().Hour > viewModel.StartTimeOfDayAvailable.GetValueOrDefault().Hours)
            {
                this.notifier.Error(translator.T("Start date available cannot be before start day availability"));
                return false;
            }
            else if (viewModel.EndDataTime.GetValueOrDefault().Hour < viewModel.EndTimeOfDayAvailable.GetValueOrDefault().Hours)
            {
                this.notifier.Error(translator.T("End date available cannot be before end day availability"));
                return false;
            }
            else if (viewModel.StartDateTime.GetValueOrDefault().Hour > viewModel.StartTimeOfDayAvailable.GetValueOrDefault().Hours)
            {
                this.notifier.Error(translator.T("Start time cannot be after end time"));
                return false;
            }
            else if((viewModel.StartDateTime == viewModel.EndDataTime) || (viewModel.StartTimeOfDayAvailable == viewModel.EndTimeOfDayAvailable))
            {
                this.notifier.Error(translator.T("Start time cannot be the same as end time"));
                return false;
            }

            return true;
        }

        //
        // GET: /Voucher/SiteVoucherCodes/Edit/5

        public ActionResult Edit(Guid id)
        {
            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.EditVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to create voucher codes"));
                return new HttpUnauthorizedResult();
            }

            var dbModel = this.siteVoucherService.Get(id);
            VoucherViewModel viewModel = dbModel.ToViewModel(this.currentSite.ExternalSiteId);

            return View(viewModel);
        }

        //
        // POST: /Voucher/SiteVoucherCodes/Edit/5

        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditPost(Guid id, VoucherViewModel viewModel)
        {
            var areValid = AreValidDates(viewModel);
            if (!areValid)
            {
                return View(viewModel);
            }

            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.EditVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to create voucher codes"));
                return new HttpUnauthorizedResult();
            }

            try
            {
                var currentVoucher= this.siteVoucherService.GetByExpression(e => e.Id == viewModel.Id);

                var voucherInDB = this.siteVoucherService.GetByExpression(e => e.VoucherCode == viewModel.VoucherCode);
                
                if (voucherInDB != null && voucherInDB.Id != id)
                {
                    this.notifier.Error(translator.T("Voucher Code already exists. choose another name."));
                    return View(viewModel);
                }

                if (viewModel.StartDateTime > viewModel.EndDataTime)
                {
                    this.notifier.Error(translator.T("Start date time should be less than End date time"));
                    return View(viewModel);
                }

                currentVoucher.UpdateFromViewModel(viewModel);
                this.siteVoucherService.Update(currentVoucher);

                this.notifier.Notify(this.translator.T("The voucher code is modified"));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.notifier.Error(translator.T(ex.Message));
                return View(viewModel);
            }
        }

        public ActionResult Remove(Guid id) 
        {
            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.DeleteVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to delete voucher codes"));
                return new HttpUnauthorizedResult();
            }

            var currentVoucher = this.siteVoucherService.GetByExpression(e => e.Id == id);

            if (currentVoucher != null) 
            {
                this.siteVoucherService.Delete(currentVoucher);
            }

            this.notifier.Notify(translator.T("{0} has been removed.", currentVoucher.VoucherCode));

            return this.RedirectToAction("Index");
        }

        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeletePost(Guid id, VoucherViewModel viewModel)
        {
            if (!this.authorizer.AuthorizeAll(VoucherCodesFeature.HasVoucherCodesFeature, UserPermissions.DeleteVoucherCodes))
            {
                this.notifier.Error(translator.T("You do not have permission to delete voucher codes"));
                return new HttpUnauthorizedResult();
            }

            try
            {
                var model = this.siteVoucherService.Get(viewModel.Id);
                this.siteVoucherService.Delete(model);
                //this.siteVoucherService.Delete(convertToModel(new List<VoucherViewModel> { viewModel })[0]);
                this.notifier.Notify(this.translator.T("The voucher code is deleted"));

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                this.notifier.Error(translator.T(ex.Message));
                return View(viewModel);
            }
        }

        //public JsonResult List()
        //{
        //    if (!this.authorizer.Authorize(UserPermissions.ViewVoucherCodes))
        //    {
        //        this.notifier.Error(translator.T("You do not have permission to view voucher codes"));
        //        return Json(Enumerable.Empty<object>(), JsonRequestBehavior.AllowGet);
        //    }
        //    if(!this.currentSite.Available)
        //        return null;

        //    var data = this.siteVoucherService.List().ToList<MyAndromeda.Data.DataWarehouse.Models.Voucher>(); 
        //    return Json(data, JsonRequestBehavior.AllowGet);
        //}

        public JsonResult ListVoucherCodesDescriptions(string searchText, bool isactive)
        {
            var data = this.siteVoucherService.List().ToList();
            List<string> results = new List<string>();
            foreach (var item in data)
            {
                bool valid = false;
                if (isactive != null)
                {
                    if (isactive != item.Removed)
                    {
                        valid = true;
                    }
                }
                if (searchText != null)
                {
                    if (searchText.Trim() != string.Empty && (item.VoucherCode + " " + item.Description).Contains(searchText))
                    {
                        valid = true;
                    }
                    else
                    {
                        valid = false;
                    }
                }
                if (valid)
                {
                    results.Add(item.VoucherCode + " " + item.Description);
                }
            }

            return Json(results, JsonRequestBehavior.AllowGet);

        }
    }
}
