using System;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.DataWarehouse.Models;
using MyAndromeda.Logging;
using MyAndromeda.Services.Ibs.Acs;
using MyAndromeda.Services.Ibs.Models;
using System.Collections.Generic;
using MyAndromeda.Services.Ibs.Checks;

namespace MyAndromeda.Services.Ibs.Implementation
{
    public class IbsService : IIbsService
    {
        private readonly IMyAndromedaLogger logger;

        private readonly IIbsCacheSettings settings;
        private readonly ILocationService locationService;
        private readonly ICreateOrderService createOrderService;
        private readonly IOrderService orderService;
        private readonly ILoginService loginService;
        private readonly ICustomerService customerService;
        private readonly IMenuService menuService;
        private readonly IAcsOrdersForIbsService acsOrdersForIbsService;
        private readonly IIbsStoreDevice ibsStoreDevice;
        private readonly IGetPaymentTypes getPaymentTypes;
        public IbsService(IIbsCacheSettings settings,
            ILoginService loginService,
            ILocationService locationService,
            IOrderService orderService,
            ICustomerService customerService,
            IMenuService menuService,
            IAcsOrdersForIbsService acsOrdersForIbsService,
            ICreateOrderService createOrderService,
            IIbsStoreDevice ibsStoreDevice,
            IMyAndromedaLogger logger,
            IGetPaymentTypes getPaymentTypes)
        {
            this.getPaymentTypes = getPaymentTypes;
            this.logger = logger;
            this.ibsStoreDevice = ibsStoreDevice;
            this.createOrderService = createOrderService;
            this.acsOrdersForIbsService = acsOrdersForIbsService;
            this.menuService = menuService;
            this.customerService = customerService;
            this.settings = settings;
            this.loginService = loginService;
            this.locationService = locationService;
            this.orderService = orderService;
        }

        private async Task<TokenResult> GetTokenResult(int andromedaSiteId) 
        {
            var token = this.settings.GetToken(andromedaSiteId);

            if (token == null) 
            {
                token = await this.loginService.LoginAsync(andromedaSiteId);
            }

            return token;
        }

        private async Task<LocationResult> GetLocationResult(int andromedaSiteId)
        {
            //var locations = await this.locationService.LoadLocationsAsync(token);
            //var location = locations.FirstOrDefault();

            IbsStoreSettings storeSettings = await this.ibsStoreDevice.GetIbsStoreDeviceAsync(andromedaSiteId);
            var location = new LocationResult()
            {
                LocationCode = storeSettings.LocationId
            };

            return location;
        }

        public async Task<CustomerResultModel> GetCustomerAsync(int andromedaSiteId, OrderHeader orderHeader) 
        {
            TokenResult token = await this.GetTokenResult(andromedaSiteId);
            Contact email = orderHeader.Customer.Contacts == null || orderHeader.Customer.Contacts.Count == 0 
                ? null 
                : orderHeader.Customer.Contacts.First(e=> e.ContactTypeId == 0);

            if (email == null) { return null; }

            var customerRequest = new CustomerRequestModel() { Email = email.Value };
            CustomerResultModel result = await this.customerService.Get(andromedaSiteId, token, customerRequest);

            return result;
        }

        public async Task<CustomerResultModel> AddCustomerAsync(int andromedaSiteId, OrderHeader orderHeader) 
        {
            TokenResult token = await this.GetTokenResult(andromedaSiteId);
            Contact email = orderHeader.Customer.Contacts.First(e => e.ContactTypeId == 0);

            var request = new AddCustomerRequestModel()
            {
                Email = email.Value,
                Customer = orderHeader.Customer.Transform()
            };

            CustomerResultModel result = await this.customerService.Add(andromedaSiteId, token, request);

            return result;
        }

        public Task<AddOrderRequest> CreateOrderData(int andromedaSiteId, OrderHeader orderHeader, CustomerResultModel customer) 
        {
            Task<AddOrderRequest> model = this.createOrderService.CreateOrderRequestModelAsync(andromedaSiteId, orderHeader, customer);   

            return model;
        }

        public async Task<MenuResult> GetMenuAsync(int andromedaSiteId)
        {
            TokenResult token = await this.GetTokenResult(andromedaSiteId);
            
            LocationResult location = await this.GetLocationResult(andromedaSiteId);
            
            MenuResult menu = await this.menuService.GetMenu(andromedaSiteId, token, location);

            return menu;
        }

        public async Task<Locations> GetLocations(int andromedaSiteId)
        {
            TokenResult token = await this.GetTokenResult(andromedaSiteId);

            Locations locations = await this.locationService.LoadLocationsAsync(andromedaSiteId, token);

            return locations;
        }

        public async Task<AddOrderResult> AddOrderAsync(int andromedaSiteId, OrderHeader orderHeader, CustomerResultModel customer, AddOrderRequest orderRequest)
        {
            TokenResult token = await this.GetTokenResult(andromedaSiteId);

            LocationResult location = await this.GetLocationResult(andromedaSiteId);
            


            AddOrderResult orderResult = await this.orderService.SendOrder(andromedaSiteId, token, location, customer, orderRequest);

            //update acs
            await this.acsOrdersForIbsService.SaveSentOrderToIbsAsync(orderHeader, orderResult);

            this.logger.Debug(format: "IBS Order Created - {0}", args: new object[] { orderResult.Id });

            return orderResult;
        }

        public async Task<IEnumerable<PaymentTypeModel>> GetPaymentTypes(int andromedaSiteId) 
        {
            TokenResult token = await this.GetTokenResult(andromedaSiteId);

            LocationResult location = await this.GetLocationResult(andromedaSiteId);

            IEnumerable<PaymentTypeModel> result = Enumerable.Empty<PaymentTypeModel>();

            try
            {
                result = await this.getPaymentTypes.List(andromedaSiteId, token, location);
            }
            catch (Exception e)
            {
                this.logger.Error(e);
                throw;
            }

            return result;
        }

    }
}