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

        private DateTime timeStamp;
        
        private DateTime orderPlacedTime;
        public DateTime? OrderPlacedTime 
        {
            get { return this.orderPlacedTime; }
            set 
            {
                var ticks = value.GetValueOrDefault().Ticks;
                this.orderPlacedTime = new DateTime(ticks, DateTimeKind.Utc);
            }
        }
        private DateTime orderWantedTime;
        public DateTime? OrderWantedTime 
        {
            get { return this.orderWantedTime; }
            set 
            {
                var ticks = value.GetValueOrDefault().Ticks;
                this.orderWantedTime = new DateTime(ticks, DateTimeKind.Utc); //DateTime.SpecifyKind(value.GetValueOrDefault(), DateTimeKind.Utc); 
            }
        }

        public DateTime TimeStamp 
        {
            get 
            {
                return this.timeStamp;
            }
            set 
            {
                this.timeStamp = new DateTime(value.Ticks, DateTimeKind.Utc);
            }
        }

        public string OrderPlacedTimeLocalString { get; set; }

        public string OrderWantedTimeLocalString { get; set; }

        public Guid? CustomerId { get; set; }

        public string Phone { get; set; }

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
        public string Email { get; set; }

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
                FinalPrice = header.FinalPrice + header.DeliveryCharge,
                PayType = header.paytype,
                TimeStamp = header.TimeStamp.ToUniversalTime(),
                OrderPlacedTime = dateServices.ConvertToLocalFromUtc(header.OrderPlacedTime.GetValueOrDefault().ToUniversalTime()),  
                OrderPlacedTimeLocalString = dateServices.ConvertToLocalString(header.OrderPlacedTime.GetValueOrDefault(), format: "g"),
                OrderWantedTime =  dateServices.ConvertToLocalFromUtc(header.OrderWantedTime.GetValueOrDefault().ToUniversalTime()),
                OrderWantedTimeLocalString = dateServices.ConvertToLocalString(header.OrderWantedTime.GetValueOrDefault(header.OrderPlacedTime.GetValueOrDefault()), format: "g"),
                TotalTax = header.TotalTax,
                FirstName = header.Customer.FirstName,
                LastName = header.Customer.LastName,                
                OrderType = header.OrderType,
                Status = header.Status                
            };
            if (header.OrderStatu != null) {
                orderHeader.OrderStatus = new OrderStatus { Description = header.OrderStatu.Description, Id = header.OrderStatu.Id };
            }
            if (header.Customer.Contacts != null && header.Customer.Contacts.Count > 0) 
            {
                var email = header.Customer.Contacts.FirstOrDefault(e=> e.ContactTypeId == 0);
                if (email == null) 
                { 
                    orderHeader.Email = "None provided"; 
                } 
                else
                { 
                    orderHeader.Email = email.Value;
                }

                var phone = header.Customer.Contacts.FirstOrDefault(e => e.ContactTypeId == 1);

                if (phone == null)
                {
                    orderHeader.Phone = "None provided";
                }
                else 
                {
                    orderHeader.Phone = phone.Value;
                }
            }
            return orderHeader;
        }
    }
}