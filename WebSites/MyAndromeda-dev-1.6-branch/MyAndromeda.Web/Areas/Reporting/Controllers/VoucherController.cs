using MyAndromeda.Data.DataWarehouse.Vouchers;
using MyAndromeda.Export;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Services.Vouchers;
using MyAndromeda.Services.Vouchers.Services;
using MyAndromeda.Web.Areas.Reporting.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using MyAndromeda.Web.Areas.Voucher.Models;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class VoucherController : Controller 
    {
        private readonly IVoucherReportingDataService voucherReportService;
        private readonly ISiteVoucherService siteVoucherService;
        private readonly IWorkContext workContext;
        private readonly IAuthorizer authorizer;
        private readonly INotifier notifier;
        private readonly ITranslator translator;
        private readonly ISpreadsheetDocumentExport spreadsheetExport;

        public VoucherController(
            IVoucherReportingDataService voucherReportService,
            IWorkContext workContext,
            IAuthorizer authorizer,
            INotifier notifier,
            ITranslator translator,
            ISpreadsheetDocumentExport spreadsheetExport,
            ISiteVoucherService siteVoucherService
            )
        {
            this.workContext = workContext;
            this.authorizer = authorizer;
            this.notifier = notifier;
            this.translator = translator;
            this.voucherReportService = voucherReportService;
            this.siteVoucherService = siteVoucherService;
            this.spreadsheetExport = spreadsheetExport;
        }

        public ActionResult Index()
        {
            VoucherSummaryViewModel vm = new VoucherSummaryViewModel();
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(VoucherSummaryViewModel vm)
        {
            vm.VoucherSummary = voucherReportService.GetTotalOrdersByCode(vm.VoucherId);
            return View(vm);
        }

        public JsonResult GetVoucherCodeList()
        {
            List<VoucherViewModel> retList = new List<VoucherViewModel>();
            var data = this.siteVoucherService.List();
            if (data != null)
                retList = convertToViewModel(data.ToList());
            return Json(retList, JsonRequestBehavior.AllowGet);
        }

        private List<VoucherViewModel> convertToViewModel(List<MyAndromeda.Data.DataWarehouse.Models.Voucher> voucherModelList)
        {
            List<VoucherViewModel> retList = new List<VoucherViewModel>();
            if (voucherModelList != null)
            {
                foreach (MyAndromeda.Data.DataWarehouse.Models.Voucher obj in voucherModelList)
                {
                    VoucherViewModel addObj = new VoucherViewModel();
                    addObj.Id = obj.Id;
                    addObj.VoucherCode = obj.VoucherCode;
                    addObj.Description = obj.Description;
                    addObj.SelectedOccasions = new List<string>();
                    if (!String.IsNullOrEmpty(obj.Occasion))
                        addObj.SelectedOccasions.AddRange(obj.Occasion.Split(',').ToList());
                    addObj.MinimumOrderAmount = obj.MinimumOrderAmount;
                    addObj.MaxRepetitions = obj.MaxRepetitions;
                    addObj.Combinable = obj.Combinable;
                    addObj.StartDateTime = obj.StartDateTime;
                    addObj.EndDataTime = obj.EndDataTime;
                    addObj.SelectedAvailableOnDays = new List<string>();
                    if (!String.IsNullOrEmpty(obj.AvailableOnDays))
                        addObj.SelectedAvailableOnDays.AddRange(obj.AvailableOnDays.Split(',').ToList());
                    addObj.StartTimeOfDayAvailable = obj.StartTimeOfDayAvailable;
                    addObj.EndTimeOfDayAvailable = obj.EndTimeOfDayAvailable;
                    addObj.IsActive = !obj.Removed;
                    addObj.DiscountType = obj.DiscountType;
                    addObj.DiscountValue = obj.DiscountValue;
                    retList.Add(addObj);
                }
            }
            return retList;
        }
    }
}