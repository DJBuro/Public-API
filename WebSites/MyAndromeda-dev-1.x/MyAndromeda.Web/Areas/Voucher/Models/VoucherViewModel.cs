using MyAndromeda.Data.DataWarehouse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.Voucher.Models
{
    public class VoucherViewModel
    {
        public Guid Id { get; set; }
        public string ExternalSiteName { get; set; }
        public string VoucherCode { get; set; }
        public string Description { get; set; }
        public List<string> SelectedOccasions { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public int? MaxRepetitions { get; set; }
        public bool Combinable { get; set; }
        public System.DateTime? StartDateTime { get; set; }
        public System.DateTime? EndDataTime { get; set; }
        public List<string> SelectedAvailableOnDays { get; set; }
        public System.TimeSpan? StartTimeOfDayAvailable { get; set; }
        public System.TimeSpan? EndTimeOfDayAvailable { get; set; }
        public bool IsActive { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }

        public string StringOccasions { set; get; }
        public string StringAvailableDays { set; get; }

        //public List<SiteVoucher> SiteVouchers { get; set; }
        //public List<UsedVoucher> UsedVouchers { get; set; }
        public VoucherWebSite[] WebSites { get; set; }
    }

    public class VoucherWebSite 
    {
        public string LiveWebsite { get; set; }
        public string PreviewWebiste { get; set; }
    }

    public static class VoucherViewModelExtensions
    {
        public static void UpdateFromViewModel(this Data.DataWarehouse.Models.Voucher voucher, VoucherViewModel viewModel)
       {
            //voucher.Id = viewModel.Id;
            voucher.VoucherCode = viewModel.VoucherCode;
            voucher.Description = viewModel.Description;
            voucher.Occasion = viewModel.SelectedOccasions != null ? string.Join(",", viewModel.SelectedOccasions) : string.Empty;
            voucher.MinimumOrderAmount = viewModel.MinimumOrderAmount;
            voucher.MaxRepetitions = viewModel.MaxRepetitions;
            voucher.Combinable = viewModel.Combinable;
            voucher.StartDateTime = viewModel.StartDateTime;
            voucher.EndDataTime = viewModel.EndDataTime;
            if (viewModel.SelectedAvailableOnDays != null)
            {
                if (viewModel.SelectedAvailableOnDays.Distinct().Count() == 7)
                {
                    voucher.AvailableOnDays = "All";
                }
                else {
                    voucher.AvailableOnDays = string.Join(",", viewModel.SelectedAvailableOnDays);
                }
            }
            else
            {
                voucher.AvailableOnDays = string.Empty;
            }
            voucher.StartTimeOfDayAvailable = viewModel.StartTimeOfDayAvailable;
            voucher.EndTimeOfDayAvailable = viewModel.EndTimeOfDayAvailable;
            voucher.Active = viewModel.IsActive;
            voucher.DiscountType = viewModel.DiscountType;
            voucher.DiscountValue = viewModel.DiscountValue;
            if (viewModel.DiscountType != null && viewModel.DiscountType.ToLower().Equals("percentage", StringComparison.InvariantCultureIgnoreCase))
                voucher.DiscountValue = ((decimal)100 - viewModel.DiscountValue);
        }

        public static VoucherViewModel ToViewModel(this Data.DataWarehouse.Models.Voucher voucher, string externalSiteId, VoucherWebSite[] websites = null)
        {
            string days = "Monday,Tuesday,Wednesday,Thursday,Friday,Saturday,Sunday";
            VoucherViewModel model = new VoucherViewModel();

            model.Id = voucher.Id;
            model.VoucherCode = voucher.VoucherCode;
            model.Description = voucher.Description;
            model.SelectedOccasions = voucher.Occasion.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            model.MinimumOrderAmount = voucher.MinimumOrderAmount;
            model.MaxRepetitions = voucher.MaxRepetitions;
            model.Combinable = voucher.Combinable;
            model.StartDateTime = voucher.StartDateTime;
            model.EndDataTime = voucher.EndDataTime;
            model.SelectedAvailableOnDays = string.IsNullOrEmpty(voucher.AvailableOnDays) ? null : (voucher.AvailableOnDays.ToLower().Equals("all") ? days.Split(new[] { ',' }).ToList() : voucher.AvailableOnDays.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList());
            model.StartTimeOfDayAvailable = voucher.StartTimeOfDayAvailable;
            model.EndTimeOfDayAvailable = voucher.EndTimeOfDayAvailable;
            model.IsActive = voucher.Active;
            model.DiscountType = voucher.DiscountType;
            model.DiscountValue = (!string.IsNullOrWhiteSpace(voucher.DiscountType) && voucher.DiscountType.Equals("percentage", StringComparison.InvariantCultureIgnoreCase)) ?
                ((decimal)100 - voucher.DiscountValue) :
                voucher.DiscountValue;
            model.StringOccasions = voucher.Occasion;
            model.StringAvailableDays = voucher.AvailableOnDays.ToLower().Equals("all") ?  days : voucher.AvailableOnDays;
            model.ExternalSiteName = externalSiteId;

            model.WebSites = websites ?? new VoucherWebSite[0];

            return model;
        }
    }
}