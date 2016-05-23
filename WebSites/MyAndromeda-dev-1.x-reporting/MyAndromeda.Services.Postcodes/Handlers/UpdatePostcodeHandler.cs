using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Logging;
using MyAndromeda.Services.Orders.Events;

namespace MyAndromeda.Services.Postcodes.Handlers
{
    public class UpdatePostcodeHandler : IOrderChangedEvent
    {
        private readonly IPostcodeService postcodeService;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly IMyAndromedaLogger logger;
        
        public UpdatePostcodeHandler(
            IPostcodeService postcodeService, 
            IOrderHeaderDataService orderHeaderDataService, 
            IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.orderHeaderDataService = orderHeaderDataService;
            this.postcodeService = postcodeService;
        }

        public string Name
        {
            get
            {
                return "Update Postcode - Order Created";
            }
        }

        public async Task<WorkLevel> OrderStatusChangedAsync(
            int andromedaSiteId, 
            OrderHeader orderHeader, 
            OrderStatu oldStatus)
        {
            //var currentState = orderHeader.GetState();

            if (orderHeader.OrderType.Equals("Collection", StringComparison.InvariantCultureIgnoreCase)) 
            {
                return WorkLevel.NoWork;
            }

            if (orderHeader.CustomerAddress == null) 
            {
                this.logger.Error("There is no customer address :( " + orderHeader.ID);
                return WorkLevel.NoWork; 
            }

            string customerAddressPostcode = orderHeader.CustomerAddress.ZipCode;
            Models.PostcodeDataResult r = null;

            System.Text.RegularExpressions.Regex regex = 
                new System.Text.RegularExpressions.Regex("^(GIR ?0AA|[A-PR-UWYZ]([0-9]{1,2}|([A-HK-Y][0-9]([0-9ABEHMNPRV-Y])?)|[0-9][A-HJKPS-UW]) ?[0-9][ABD-HJLNP-UW-Z]{2})$");

            if (!regex.IsMatch(customerAddressPostcode)) 
            {
                logger.Error("This isn't a postcode: " + customerAddressPostcode);
            }

            if (string.IsNullOrWhiteSpace(orderHeader.CustomerAddress.Latitude)) 
            {
                r = await this.postcodeService.GetDataFromPostcodeAsync(customerAddressPostcode);

                if (r != null) 
                {
                    orderHeader.CustomerAddress.Latitude = r.Result.Latitude.Substring(0,15);
                    orderHeader.CustomerAddress.Longitude = r.Result.Longitude.Substring(0, 15);
                }
            }

            if (string.IsNullOrWhiteSpace(orderHeader.Customer.Address.Lat)) 
            {
                if (orderHeader.Customer.Address.PostCode != customerAddressPostcode)
                {
                    r = await this.postcodeService.GetDataFromPostcodeAsync(customerAddressPostcode);
                }
                
                orderHeader.Customer.Address.Lat = r.Result.Latitude.Substring(0, 15);
                orderHeader.Customer.Address.Long = r.Result.Longitude.Substring(0, 15);
                
            }

            try
            {
                int changed = await this.orderHeaderDataService.SaveChangesAsync();
            }
            catch (Exception e) 
            {
                this.logger.Error(e);
                throw e;
            }
            return WorkLevel.CompletedWork;
        }
    }
}
