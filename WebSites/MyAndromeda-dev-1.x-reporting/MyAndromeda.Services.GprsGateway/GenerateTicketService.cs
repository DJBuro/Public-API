using System;
using System.Linq;
using System.Text;
using MyAndromeda.Data.DataWarehouse.Models;
using System.Threading.Tasks;
using MyAndromeda.Data.Model.AndroAdmin;
using MyAndromeda.Logging;
using System.Collections.Generic;

namespace MyAndromeda.Services.GprsGateway
{
    public class GenerateTicketService : IGenerateTicketService 
    {
        private readonly IMyAndromedaLogger logger;

        public GenerateTicketService(IMyAndromedaLogger logger) 
        {
            this.logger = logger;
        }

        public async Task<string> Generate(Store store, OrderHeader order)
        {
            var sb = new StringBuilder();
            sb.StartOrEndOfOrder();

            try
            {
                sb.Append(await this.BuildHeaderSegment(store, order));
                sb.Append(await this.BuildUpLineItemSegment(order));
                sb.Append(await this.BuildUpChargesSegment(order));
                //sb.Append(await this.BuildUpTotalSegment(order));
                sb.Append(await this.BuildCustomerSegment(order, order.Customer, order.CustomerAddress));
                sb.Append(await this.BuildUpDetailSegment(order));
                sb.Append(await this.BuildUpComments(order));
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw e;
            }
            //#\raddress line 1\raddress line 2\raddress line 3\rphone number;10001;Delivery/Collection;*\rBurnt Large\r;3;Chickens;*3.00;4.00;14.00;30.00/r;*Joe Bloggs;0123456789;\rRoad1\rPlace 3\rCounty\rPostCode;*Date;Date;Date;*No hands;I dont exist;#

            sb.StartOrEndOfOrder();

            return sb.ToString();
        }
 

 
        private Task<string> BuildUpComments(OrderHeader order)
        {
            string directions = string.Empty;

            var sb = new StringBuilder();
            
            sb.Append(order.CookingInstructions);
            sb.EndOfPart(TicketDepth.SegmentChild);
            
            if (order.OrderType.Equals(value: "Collection", comparisonType: StringComparison.InvariantCultureIgnoreCase)) 
            {
                sb.Append(string.Empty);
                sb.EndOfPart(TicketDepth.SegmentChild);
            }
            else
            {
                if (order.Customer != null && order.Customer.Address != null) 
                {
                    if (!string.IsNullOrWhiteSpace(order.Customer.Address.Directions))
                    {
                        sb.Append(directions);
                    }
                    else
                    {
                        sb.Append(string.Empty);
                    }
                }

                sb.EndOfPart(TicketDepth.SegmentChild);
            }

            sb.EndOfPart(TicketDepth.Segment);

            return Task.FromResult(sb.ToString());
        }
 
        private Task<string> BuildUpDetailSegment(OrderHeader order)
        {
            string orderedAt = order.TimeStamp.ToString(format: "g");
            string wantedAt = order.OrderWantedTime.GetValueOrDefault().ToString(format: "g");
            string acceptedFor = order.OrderWantedTime.GetValueOrDefault().ToString(format: "g");

            var sb = new StringBuilder();

            sb.Append(orderedAt).EndOfPart(TicketDepth.SegmentChild);
            sb.Append(wantedAt).EndOfPart(TicketDepth.SegmentChild);
            sb.Append(acceptedFor).EndOfPart(TicketDepth.SegmentChild);
            sb.EndOfPart(TicketDepth.Segment);

            return Task.FromResult(sb.ToString());
        }
 
        private async Task<string> BuildUpChargesSegment(OrderHeader order)
        {
            var sb = new StringBuilder();
            ICollection<OrderPayment> charges = await order.OrderPayments.LoadAsync();

            decimal deliveryCharge = order.DeliveryCharge;
            sb.Append(string.Format(format: "{0:0.00}", arg0: deliveryCharge));
            sb.EndOfPart(TicketDepth.SegmentChild);

            int? paymentCharge = (charges.Sum(e => e.PaymentCharge) / 100);
            sb.Append(string.Format(format: "{0:0.00}", arg0: paymentCharge));
            sb.EndOfPart(TicketDepth.SegmentChild);

            ICollection<OrderDiscount> discountRows = await order.OrderDiscounts.LoadAsync();
            decimal discount = discountRows.Sum(e => ((decimal)e.DiscountAmount) / 100);
            sb.Append(string.Format(format: "{0:0.00}", arg0: discount));
            sb.EndOfPart(TicketDepth.SegmentChild);

            decimal finalFinalPrice = order.FinalPrice + order.DeliveryCharge;
            sb.Append(finalFinalPrice.ToString());
            sb.EndOfPart(TicketDepth.SegmentChild);

            sb.EndOfPart(TicketDepth.Segment);

            return sb.ToString();
        }
 
        private Task<string> BuildHeaderSegment(Store store, OrderHeader order)
        {
            var sb = new StringBuilder();

            //should already be part of the result hence the lack of await. 
            Customer customer = order.Customer;
            CustomerAddress addresss = order.CustomerAddress;

            string storeAddress = this.BuildStoreAddress(store);

            sb.Append(storeAddress).EndOfPart(TicketDepth.SegmentChild);
            sb.Append(order.RamesesOrderNum).EndOfPart(TicketDepth.SegmentChild);
            sb.Append(order.OrderType).EndOfPart(TicketDepth.SegmentChild);
            sb.EndOfPart(TicketDepth.Segment);

            return Task.FromResult(sb.ToString());
        }
 
        private async Task<string> BuildCustomerSegment(OrderHeader order, Customer customer, CustomerAddress address)
        {
            if (address == null) { return "".EndOfPart(TicketDepth.SegmentChild); }

            ICollection<Contact> contacts = await customer.Contacts.LoadAsync();
            Contact phone = contacts.FirstOrDefault(e => e.ContactType.Name.Equals(value: "Mobile", comparisonType: StringComparison.InvariantCultureIgnoreCase));

            var sb = new StringBuilder();
            
            //part 1 
            sb.Append(customer.FirstName);
            sb.EndOfPart(TicketDepth.SegmentChild);
            
            if(phone == null)
            {
                //part 2a
                sb.Append(string.Empty);
                sb.EndOfPart(TicketDepth.SegmentChild);
            }
            else
            {
                //part 2b
                sb.Append(phone.Value);
                sb.EndOfPart(TicketDepth.SegmentChild);
            }

            //part 3a
            if (order.OrderType.IndexOf(value: "Collection", comparisonType: StringComparison.InvariantCultureIgnoreCase)>= 0) 
            {
                sb.Append(value: "Collecting...");
                sb.EndOfPart(TicketDepth.SegmentChild);
                sb.EndOfPart(TicketDepth.Segment);

                return sb.ToString();
            }

            //part 3b
            if(address == null)
            {
                sb.Append(string.Empty);
                sb.EndOfPart(TicketDepth.SegmentChild);
                sb.EndOfPart(TicketDepth.Segment);

                return sb.ToString();
            }
                
            if(!string.IsNullOrWhiteSpace(address.RoadNum) || !string.IsNullOrWhiteSpace(address.RoadName))
            {
                if(!string.IsNullOrWhiteSpace(address.RoadNum))
                {
                    sb.Append(address.RoadNum);

                    if (!string.IsNullOrWhiteSpace(address.RoadName)) 
                    {
                        sb.AddSpace();
                    }
                }

                if(!string.IsNullOrWhiteSpace(address.RoadName))
                {
                    sb.Append(address.RoadName);
                }
                sb.StartNewLine();
            }
            if(!string.IsNullOrWhiteSpace(address.City))
            {
                sb.Append(address.City).StartNewLine();
            }
            if(!string.IsNullOrWhiteSpace(address.State))
            {
                sb.Append(address.State).StartNewLine();
            }
            if(!string.IsNullOrWhiteSpace(address.ZipCode))
            {
                sb.Append(address.ZipCode).StartNewLine();
            }
            if(!string.IsNullOrWhiteSpace(address.Country))
            {
                sb.Append(address.Country).StartNewLine();
            }

            sb.EndOfPart(TicketDepth.SegmentChild);
            sb.EndOfPart(TicketDepth.Segment);
            
            return sb.ToString();
        }

        private string BuildStoreAddress(Store store) 
        {
            Data.Model.AndroAdmin.Address address = store.Address;

            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(address.RoadNum) || !string.IsNullOrWhiteSpace(address.RoadName))
            {
                if (!string.IsNullOrWhiteSpace(address.RoadNum))
                {
                    sb.Append(address.RoadNum);
                }
                if (!string.IsNullOrWhiteSpace(address.RoadName))
                {
                    sb.Append(address.RoadName);
                }

                sb.StartNewLine();
            }
            if (!string.IsNullOrWhiteSpace(address.Town))
            {
                sb.Append(address.Town).StartNewLine();
            }
            if (!string.IsNullOrWhiteSpace(address.State))
            {
                sb.Append(address.State).StartNewLine();
            }
            if (!string.IsNullOrWhiteSpace(address.PostCode))
            {
                sb.Append(address.PostCode).StartNewLine();
            }
            if (!string.IsNullOrWhiteSpace(store.Telephone)) 
            {
                sb.Append(store.Telephone).StartNewLine();
            }

            return sb.ToString();
        }

        private async Task<string> BuildUpLineItemSegment(OrderHeader order)
        {
            var sb = new StringBuilder();

            ICollection<OrderLine> orderLines = await order.OrderLines.LoadAsync();
            
            foreach(var orderline in orderLines)
            {
                sb.Append(await this.BuildUpLineItem(orderline));
            }

            sb.EndOfPart(TicketDepth.Segment);

            return sb.ToString();
        }

        private async Task<string> BuildUpLineItem(OrderLine orderLine) 
        {
            var sb = new StringBuilder();

            //part 1 
            sb.Append(orderLine.Cat1 ?? "");
            if(!string.IsNullOrWhiteSpace(orderLine.Cat1) && !string.IsNullOrWhiteSpace(orderLine.Cat2))
            {
                sb.AddSpace();
            }
            sb.Append(orderLine.Cat2 ?? "");
            sb.StartNewLine();
            sb.EndOfPart(TicketDepth.SegmentChild);
            //part 2 
            sb.Append(orderLine.Qty.GetValueOrDefault(defaultValue: 1).ToString());
            sb.EndOfPart(TicketDepth.SegmentChild);
            //part 3
            sb.Append(orderLine.Description);
            sb.EndOfPart(TicketDepth.SegmentChild);
            //part 4 
            sb.Append(((decimal)orderLine.Price.GetValueOrDefault()) / 100);
            sb.EndOfPart(TicketDepth.SegmentChild);
            //part 5 

            //comma separated
            string modifiers = await BuildUpModifiers(orderLine);
            sb.Append(modifiers);
            sb.EndOfPart(TicketDepth.SegmentChild);
            
            return sb.ToString();
        }

        private async Task<string> BuildUpModifiers(OrderLine orderLine) 
        {
            ICollection<modifier> modifiers = await orderLine.modifiers.LoadAsync();

            var sb = new StringBuilder();

            foreach (var modifier in modifiers.Where(e=> !e.Removed.GetValueOrDefault()))
            {
                //todo 
                sb.AppendFormat(format: "+{0} X {1}", arg0: modifier.Qty, arg1: modifier.Description);
                sb.EndOfPart(TicketDepth.ContentChild);
            }

            foreach (var modifier in modifiers.Where(e => e.Removed.GetValueOrDefault()))
            {
                sb.AppendFormat(format: "-{0} X {1}", arg0: modifier.Qty, arg1: modifier.Description);
                sb.EndOfPart(TicketDepth.ContentChild);
            }

            return sb.ToString();
        }
    }
}