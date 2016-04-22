using MyAndromeda.Framework.Dates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MyAndromeda.Web.Areas.OrderManagement.Models
{
    public class OrderHeaderViewModel
    {
        public System.Guid Id { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public Nullable<System.Guid> CustomerID { get; set; }
        public string OrderCurrency { get; set; }
        public string OrderType { get; set; }

        public int ApplicationID { get; set; }
        public string ApplicationName { get; set; }
        public string RamesesOrderNum { get; set; }
        public string ExternalOrderRef { get; set; }
        public string ExternalSiteID { get; set; }
        public string SiteName { get; set; }
        public System.Guid ACSOrderId { get; set; }
        public string Paytype { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal TotalTax { get; set; }
        public decimal DeliveryCharge { get; set; }
        public bool PriceIncludeTax { get; set; }
        public string PartnerName { get; set; }
        public bool Cancelled { get; set; }
        public int Status { get; set; }
        public int ACSErrorCodeNumber { get; set; }

        public Nullable<System.DateTime> OrderPlacedTime { get; set; }
        public Nullable<System.DateTime> OrderWantedTime { get; set; }
        public string OrderPlacedTimeLocalString { get; set; }
        public string OrderWantedTimeLocalString { get; set; }

        public int IsFailed
        {
            get
            {
                return (ACSErrorCodeNumber != 0) ? 1 : 0;
            }
        }

        public int IsSuccess
        {
            get
            {
                return (ACSErrorCodeNumber == 0) ? 1 : 0;
            }
        }

        public int IsCancelled { get { return Status == 6 ? 1 : 0; } }
        public string DestinationDevice { get; set; }
        public Nullable<System.Guid> CustomerAddressID { get; set; }
        public string ACSServer { get; set; }

        public CustomerDAO Customer { get; set; }
        public CustomerAddress CustomerAddress { get; set; }
        public ACSErrorCode ACSErrorCode { get; set; }
        public ICollection<OrderLineDAO> OrderLines { get; set; }
        public ICollection<UsedVoucher> UsedVouchers { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }

    public class CustomerDAO
    {
        public System.Guid ID { get; set; }
        public string Title { get; set; }
        public string FullName { get { return string.Format("{0} {1}", this.FirstName, this.LastName); } }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { set; get; }
        public string Email { set; get; }
        public Nullable<int> AddressId { get; set; }
        public int ACSAplicationId { get; set; }
        public System.DateTime RegisteredDateTime { get; set; }
        public Nullable<System.Guid> CustomerAccountId { get; set; }
    }

    public class ACSErrorCode
    {
        public int ErrorCode { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }
    }

    public class CustomerAddress
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> CustomerKey { get; set; }
        public string RoadNum { get; set; }
        public string RoadName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
    }

    public class OrderLineDAO
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> OrderHeaderID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Cat1 { get; set; }
        public string Cat2 { get; set; }
        public ICollection<modifier> Modifiers { get; set; }
    }

    public partial class modifier
    {
        public System.Guid ID { get; set; }
        public Nullable<System.Guid> OrderLineID { get; set; }
        public Nullable<int> ProductID { get; set; }
        public string Description { get; set; }
        public Nullable<int> Qty { get; set; }
        public Nullable<bool> Removed { get; set; }
        public Nullable<int> Price { get; set; }
        public OrderLineDAO OrderLine { get; set; }
    }

    public class UsedVoucher
    {
        public System.Guid VoucherId { get; set; }
        public System.Guid CustomerId { get; set; }
        public System.Guid OrderId { get; set; }
        public VoucherCode Voucher { get; set; }
    }

    public class VoucherCode
    {
        public System.Guid Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public List<string> Occasions { get; set; }
        public decimal? MinimumOrderAmount { get; set; }
        public int? MaxRepetitions { get; set; }
        public bool Combinable { get; set; }
        public System.DateTime? StartDateTime { get; set; }
        public System.DateTime? EndDataTime { get; set; }
        public List<string> AvailableOnDays { get; set; }
        public System.TimeSpan? StartTimeOfDayAvailable { get; set; }
        public System.TimeSpan? EndTimeOfDayAvailable { get; set; }
        public bool IsActive { get; set; }
        public bool IsRemoved { get; set; }
        public string DiscountType { get; set; }
        public decimal DiscountValue { get; set; }

        public string stringOccasions { set; get; }
        public string stringAvailableDays { set; get; }
    }

    public class OrderStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public static class OrderHeaderViewModelExtensions
    {
        public static void UpdateFromViewModel(this Data.DataWarehouse.Models.OrderHeader orderHeader, OrderHeaderViewModel viewModel)
        {
            orderHeader.ID = viewModel.Id;
            orderHeader.TimeStamp = viewModel.TimeStamp;
            orderHeader.CustomerID = viewModel.CustomerID;
            orderHeader.OrderCurrency = viewModel.OrderCurrency;
            orderHeader.OrderType = viewModel.OrderType;
            orderHeader.OrderPlacedTime = viewModel.OrderPlacedTime;
            orderHeader.OrderWantedTime = viewModel.OrderWantedTime;
            orderHeader.ApplicationID = viewModel.ApplicationID;
            orderHeader.ApplicationName = viewModel.ApplicationName;
            orderHeader.RamesesOrderNum = Convert.ToInt32(viewModel.RamesesOrderNum);
            orderHeader.ExternalOrderRef = viewModel.ExternalOrderRef;
            orderHeader.ExternalSiteID = viewModel.ExternalSiteID;
            orderHeader.SiteName = viewModel.SiteName;
            orderHeader.ACSOrderId = viewModel.ACSOrderId;
            orderHeader.paytype = viewModel.Paytype;
            orderHeader.FinalPrice = viewModel.FinalPrice;
            orderHeader.TotalTax = viewModel.TotalTax;
            orderHeader.DeliveryCharge = viewModel.DeliveryCharge;
            orderHeader.PriceIncludeTax = viewModel.PriceIncludeTax;
            orderHeader.PartnerName = viewModel.PartnerName;
            orderHeader.Cancelled = viewModel.Cancelled;
            orderHeader.Status = viewModel.Status;
            orderHeader.ACSErrorCode = viewModel.ACSErrorCodeNumber;
            orderHeader.DestinationDevice = viewModel.DestinationDevice;
            orderHeader.CustomerAddressID = viewModel.CustomerAddressID;
            orderHeader.ACSServer = viewModel.ACSServer;

            if (orderHeader.Customer.Contacts.Where(e=>e.ContactType.Name.Equals(value: "Mobile", comparisonType: StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault()!=null)
            {
                orderHeader.Customer.Contacts.Where(e => e.ContactType.Name.Equals(value: "Mobile", comparisonType: StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value = viewModel.Customer.Mobile;
            }
            if (orderHeader.Customer.Contacts.Where(e => e.ContactType.Name.Equals(value: "Email", comparisonType: StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault() != null)
            {
                orderHeader.Customer.Contacts.Where(e => e.ContactType.Name.Equals(value: "Email", comparisonType: StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value = viewModel.Customer.Email;
            }
        }

        public static OrderHeaderViewModel ToViewModel(this Data.DataWarehouse.Models.OrderHeader oh, IDateServices dateServices)
        {
            OrderHeaderViewModel viewModel = new OrderHeaderViewModel();
            viewModel.Id = oh.ID;
            viewModel.TimeStamp = oh.TimeStamp;
            viewModel.CustomerID = oh.CustomerID;
            viewModel.OrderCurrency = oh.OrderCurrency;
            viewModel.OrderType = oh.OrderType;
            viewModel.OrderPlacedTime = dateServices.ConvertToLocalFromUtc(oh.OrderPlacedTime);
            viewModel.OrderPlacedTimeLocalString = dateServices.ConvertToLocalString(oh.OrderPlacedTime.GetValueOrDefault());
            viewModel.OrderWantedTime = dateServices.ConvertToLocalFromUtc(oh.OrderWantedTime);
            viewModel.OrderWantedTimeLocalString = dateServices.ConvertToLocalString(oh.OrderWantedTime.GetValueOrDefault(viewModel.OrderPlacedTime.GetValueOrDefault()));

            viewModel.ApplicationID = oh.ApplicationID;
            viewModel.ApplicationName = oh.ApplicationName;
            viewModel.RamesesOrderNum = oh.RamesesOrderNum.ToString();
            viewModel.ExternalOrderRef = oh.ExternalOrderRef;
            viewModel.ExternalSiteID = oh.ExternalSiteID;
            viewModel.SiteName = oh.SiteName;
            viewModel.ACSOrderId = oh.ACSOrderId;
            viewModel.Paytype = oh.paytype;
            viewModel.FinalPrice = oh.FinalPrice;
            viewModel.TotalTax = oh.TotalTax;
            viewModel.DeliveryCharge = oh.DeliveryCharge;
            viewModel.PriceIncludeTax = oh.PriceIncludeTax;
            viewModel.PartnerName = oh.PartnerName;
            viewModel.Cancelled = oh.Cancelled;
            viewModel.Status = oh.Status;
            viewModel.ACSErrorCodeNumber = oh.ACSErrorCode;
            viewModel.DestinationDevice = oh.DestinationDevice;
            viewModel.CustomerAddressID = oh.CustomerAddressID;
            viewModel.ACSServer = oh.ACSServer;
            if (oh.ACSErrorCode1 != null)
            {
                viewModel.ACSErrorCode = new ACSErrorCode()
                {
                    ErrorCode = oh.ACSErrorCode1.ErrorCode,
                    ShortDescription = oh.ACSErrorCode1.ShortDescription,
                    LongDescription = oh.ACSErrorCode1.LongDescription
                };
            }
            viewModel.Customer = new CustomerDAO()
            {
                ID = oh.Customer.ID,
                Title = oh.Customer.Title,
                FirstName = oh.Customer.FirstName,
                LastName = oh.Customer.LastName,
                AddressId = oh.Customer.AddressId,
                ACSAplicationId = oh.Customer.ACSAplicationId,
                RegisteredDateTime = oh.Customer.RegisteredDateTime,
                CustomerAccountId = oh.Customer.CustomerAccountId,
                Mobile = oh.Customer.Contacts.Where(e => e.ContactType.Name.Equals("Mobile", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault() != null ? oh.Customer.Contacts.Where(e => e.ContactType.Name.Equals("Mobile", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value : string.Empty,
                Email = oh.Customer.Contacts.Where(e => e.ContactType.Name.Equals("Email", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault() != null ? oh.Customer.Contacts.Where(e => e.ContactType.Name.Equals("Email", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault().Value : string.Empty
            };

            if (oh.CustomerAddress != null)
            {
                viewModel.CustomerAddress = new CustomerAddress()
                {
                    ID = oh.CustomerAddress.ID,
                    CustomerKey = oh.CustomerAddress.CustomerKey,
                    RoadNum = oh.CustomerAddress.RoadNum,
                    RoadName = oh.CustomerAddress.RoadName,
                    City = oh.CustomerAddress.City,
                    State = oh.CustomerAddress.State,
                    ZipCode = oh.CustomerAddress.ZipCode,
                    Country = oh.CustomerAddress.Country
                };
            }
            //this should be ToViewModel
            if (oh.OrderLines != null && oh.OrderLines.Count > 0)
            {
                viewModel.OrderLines = new List<OrderLineDAO>();
                foreach (Data.DataWarehouse.Models.OrderLine line in oh.OrderLines)
                {
                    var orderlineObj = new OrderLineDAO();
                    orderlineObj.Modifiers = new List<modifier>();
                    orderlineObj.ID = line.ID;
                    orderlineObj.OrderHeaderID = line.OrderHeaderID;
                    orderlineObj.ProductID = line.ProductID;
                    orderlineObj.Description = line.Description;
                    orderlineObj.Qty = line.Qty;
                    //orderlineObj.Price = line.Price != null ? (((double)line.Price) / 100) : 0;
                    orderlineObj.Cat1 = line.Cat1;
                    orderlineObj.Cat2 = line.Cat2;

                    if (line.modifiers != null && line.modifiers.Count > 0)
                    {
                        //this should be ToViewModel
                        foreach (Data.DataWarehouse.Models.modifier modifierObj in line.modifiers)
                        {
                            var obj = new modifier();
                            obj.ID = modifierObj.ID;
                            obj.OrderLineID = modifierObj.OrderLineID;
                            obj.ProductID = modifierObj.ProductID;
                            obj.Description = modifierObj.Description;
                            obj.Qty = modifierObj.Qty;
                            obj.Removed = modifierObj.Removed;
                            obj.Price = modifierObj.Price;
                            orderlineObj.Modifiers.Add(obj);
                        }
                    }

                    orderlineObj.Price = ((decimal)line.Price.GetValueOrDefault() +
                        //modifier price is the total, quantity x price ... ignore the semantics 
                        line.modifiers.Sum(m => (decimal)m.Price.GetValueOrDefault(defaultValue: 0) * (decimal)line.Qty.GetValueOrDefault(defaultValue: 0)));
                    orderlineObj.Price = orderlineObj.Price / 100;

                    viewModel.OrderLines.Add(orderlineObj);
                }
            }
            //this should be to view model
            if (oh.UsedVouchers != null && oh.UsedVouchers.Count() > 0)
            {
                viewModel.UsedVouchers = new List<UsedVoucher>();
                foreach (Data.DataWarehouse.Models.UsedVoucher uv in oh.UsedVouchers)
                {
                    var uvo = new UsedVoucher();
                    uvo.CustomerId = uv.CustomerId;
                    uvo.OrderId = uv.OrderId;
                    uvo.VoucherId = uv.VoucherId;
                    uvo.Voucher = new VoucherCode();
                    uvo.Voucher.Id = uv.Voucher.Id;
                    uvo.Voucher.Code = uv.Voucher.VoucherCode;
                    uvo.Voucher.Description = uv.Voucher.Description;
                    uvo.Voucher.Occasions = uv.Voucher.Occasion != null ? new List<string>(uv.Voucher.Occasion.Split(separator: new char[] { ',' })) : new List<string>();
                    uvo.Voucher.MinimumOrderAmount = uv.Voucher.MinimumOrderAmount;
                    uvo.Voucher.MaxRepetitions = uv.Voucher.MaxRepetitions;
                    uvo.Voucher.Combinable = uv.Voucher.Combinable;
                    uvo.Voucher.StartDateTime = uv.Voucher.StartDateTime;
                    uvo.Voucher.EndDataTime = uv.Voucher.EndDataTime;
                    uvo.Voucher.AvailableOnDays = uv.Voucher.AvailableOnDays != null ? new List<string>(uv.Voucher.AvailableOnDays.Split(separator: new char[] { ',' })) : new List<string>();
                    uvo.Voucher.StartTimeOfDayAvailable = uv.Voucher.StartTimeOfDayAvailable;
                    uvo.Voucher.EndTimeOfDayAvailable = uv.Voucher.EndTimeOfDayAvailable;
                    uvo.Voucher.IsActive = !uv.Voucher.Removed;
                    uvo.Voucher.DiscountType = uv.Voucher.DiscountType;
                    uvo.Voucher.DiscountValue = uv.Voucher.DiscountValue;
                    uvo.Voucher.stringOccasions = uv.Voucher.Occasion;
                    uvo.Voucher.stringAvailableDays = uv.Voucher.AvailableOnDays;
                    uvo.Voucher.IsRemoved = uv.Voucher.Removed;
                    viewModel.UsedVouchers.Add(uvo);
                }
            }
            if (oh.OrderStatu != null)
            {
                viewModel.OrderStatus = new OrderStatus();
                viewModel.OrderStatus.Id = oh.OrderStatu.Id;
                viewModel.OrderStatus.Description = oh.OrderStatu.Description;
            }
            return viewModel;
        }
    }
}