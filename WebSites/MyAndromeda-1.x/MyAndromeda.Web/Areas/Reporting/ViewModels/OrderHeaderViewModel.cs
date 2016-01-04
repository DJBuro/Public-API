using System;
using System.Linq;
using MyAndromeda.Core;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Web.Areas.Reporting.ViewModels
{
    public class OrderHeaderViewModel
    {

        public Guid ID { get; set; }
        public Guid ACSOrderId { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime OrderPlacedTime { get; set; }

        public Guid? CustomerId { get; set; }

        public int WeekNumber 
        {
            get { return this.OrderPlacedTime.GetWeekNumber(); } 
        }

        public decimal DeliveryCharge { get; set; }

        public decimal FinalPrice { get; set; }
        
        public string PayType { get; set; }
        
        public decimal TotalTax { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string OrderType { get; set; }
        public int Status { get; set; }

        public OrderStatus OrderStatus { get; set; }
    }

    public class OrderStatus
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }

    public static class OrderHeaderViewModelExtensions
    {
        //public static OrderHeaderViewModel ToViewModel(this OrderHeader header) 
        //{
        //    return new OrderHeaderViewModel() { 
        //        ACSOrderId = header.ACSOrderId,
        //        DeliveryCharge = header.DeliveryCharge,
        //        FinalPrice = header.FinalPrice,
        //        PayType = header.paytype,
        //        TimeStamp = DateTime.SpecifyKind(header.TimeStamp, DateTimeKind.Utc),
        //        OrderPlacedTime = DateTime.SpecifyKind(header.OrderPlacedTime.GetValueOrDefault(), DateTimeKind.Unspecified),
        //        TotalTax = header.TotalTax,

        //        FirstName = header.Customer.FirstName,
        //        LastName = header.Customer.LastName,

        //        OrderType = header.OrderType  ,
        //        CustomerId = header.CustomerID
        //    };
        //}

        public static OrderHeaderViewModel ToViewModel(this MyAndromeda.Data.DataWarehouse.Models.OrderHeader header,IDateServices dateServices)
        {           

            OrderHeaderViewModel orderHeader =  new OrderHeaderViewModel()
            {
                CustomerId = header.CustomerID,
                ID = header.ID,
                ACSOrderId = header.ACSOrderId,
                DeliveryCharge = header.DeliveryCharge,
                FinalPrice = header.FinalPrice,
                PayType = header.paytype,
                TimeStamp = DateTime.SpecifyKind(header.TimeStamp, DateTimeKind.Utc),
                OrderPlacedTime = dateServices.ConvertToLocalFromUtc(header.OrderPlacedTime.GetValueOrDefault()).GetValueOrDefault(),                
                TotalTax = header.TotalTax,
                FirstName = header.Customer.FirstName,
                LastName = header.Customer.LastName,                
                OrderType = header.OrderType,
                Status = header.Status                
            };
            if (header.OrderStatu != null) {
                orderHeader.OrderStatus = new OrderStatus { Description = header.OrderStatu.Description, Id = header.OrderStatu.Id };
            }
            return orderHeader;
        }
    }
}