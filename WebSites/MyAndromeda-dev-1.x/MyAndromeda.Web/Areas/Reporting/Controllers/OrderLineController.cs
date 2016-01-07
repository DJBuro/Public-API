using MyAndromeda.Data.DataWarehouse.Services.Orders;
using MyAndromeda.Data.DataWarehouse.Services.Customers;
using MyAndromeda.Framework;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using System;
using System.Linq;
using System.Web.Mvc;
using MyAndromeda.Framework.Dates;

namespace MyAndromeda.Web.Areas.Reporting.Controllers
{
    public class OrderLineController : Controller
    {
        private readonly IAuthorizer authorizer; 
        private readonly IOrderLineDataService orderLineDataService;
        private readonly IAcsCustomerDataService customerDataService;
        private readonly ICurrentSite currentSite;
        private readonly ISiteAddressDataService siteAddressDataService;
        private readonly IOrderHeaderDataService orderHeaderDataService;
        private readonly INotifier notifier;
        private readonly ITranslator translator;
        private readonly IWorkContext workContext;

        public OrderLineController(ICurrentSite currentSite,
            IOrderLineDataService orderLineDataService,
            IAcsCustomerDataService customerDataService,
            ISiteAddressDataService siteAddressDataService,
            IOrderHeaderDataService orderHeaderDataService,
            IAuthorizer authorizer,
            INotifier notifier,
            ITranslator translator,
            IWorkContext workContext)
        { 
            this.workContext = workContext;
            this.translator = translator;
            this.notifier = notifier;
            this.authorizer = authorizer;
            this.orderHeaderDataService = orderHeaderDataService;
            this.siteAddressDataService = siteAddressDataService;
            this.currentSite = currentSite;
            this.customerDataService = customerDataService;
            this.orderLineDataService = orderLineDataService;
        }

        //public ActionResult FindOrder(Guid acsOrderId) 
        //{
        //    if (!authorizer.Authorize(EnrollmentPermissions.BasicAcsReportsFeature))
        //    {
        //        this.notifier.Notify(translator.T(Messages.NotAuthorizedView));

        //        return new HttpUnauthorizedResult();
        //    }

        //    var orderHeader = this.orderHeaderDataService.GetByOrderId(acsOrderId);
        //    var orderLineItems = 
        //        //orderLineDataService.GetOrderedItems(acsOrderId);


        //    var customer = customerDataService.GetCustomerByAcsOrderId(acsOrderId);
        //    var storeAddress = this.siteAddressDataService.GetSiteAddress(this.currentSite.SiteId);

        //    Func<string> getStoreName = () => {
        //        if(!string.IsNullOrWhiteSpace(this.currentSite.Site.ExternalName))
        //            return this.currentSite.Site.ExternalName;

        //        if(!string.IsNullOrWhiteSpace(this.currentSite.Site.ClientSiteName))
        //            return this.currentSite.Site.ClientSiteName;

        //        return string.Empty;
        //    };

        //    var dateServices = this.workContext.DateServicesFactory();

        //    var result = new
        //    {
        //        Header = new
        //        {
        //            Name = getStoreName(),
        //            Address = new
        //            {
        //                storeAddress.RoadNum,
        //                storeAddress.RoadName,
        //                City = storeAddress.Town,
        //                storeAddress.State,
        //                ZipCode = storeAddress.Postcode,
        //                Country = storeAddress.Country.CountryName
        //            }
        //        },
        //        OrderedItems = orderLineItems.Where(e => !e.DealID.HasValue).Select(e => new
        //        {
        //            e.Description,
        //            e.Price,
        //            e.Qty,
        //            e.IsDeal,
        //            Children = orderLineItems.Where(item=> item.DealID == e.ID).OrderBy(item=> item.DealSequence).Select(item => new { 
        //                item.Description,
        //                item.Price,
        //                item.Qty
        //            })
        //        }),
        //        OrderLineAggregate = new
        //        {
        //            Sum = orderLineItems.Sum(e => e.Price),
        //            orderHeader.TotalTax,
        //            orderHeader.DeliveryCharge,
        //            orderHeader.FinalPrice
        //        },
        //        Customer = new
        //        {
        //            customer.FirstName,
        //            customer.LastName,
        //            //customer.Mobile,
        //            //customer.Email,
        //            Addresses = customer.CustomerAddresses.Select(e => new
        //            {
        //                e.RoadNum,
        //                e.RoadName,
        //                e.City,
        //                e.State,
        //                e.ZipCode,
        //                e.Country
        //            })
        //        },
        //        Delivery = new
        //        {
        //            orderHeader.OrderType,
        //            orderHeader.OrderPlacedTime,
        //            OrderPlacedTimeLocalString = dateServices.ConvertToLocalString(orderHeader.OrderPlacedTime.GetValueOrDefault()),
        //            orderHeader.OrderWantedTime,
        //            OrderWantedTimeLocalString = dateServices.ConvertToLocalString(orderHeader.OrderWantedTime.GetValueOrDefault(orderHeader.OrderPlacedTime.GetValueOrDefault()))
        //        },
        //        ReferenceIds = new
        //        {
        //            orderHeader.RamesesOrderNum,
        //            orderHeader.ExternalOrderRef
        //        }
        //    };

        //    return Json(result);
        //}
    }
}