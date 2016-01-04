using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using MyAndromeda.Logging;
using MyAndromeda.Web.Areas.DeliveryZone.Models;
using MyAndromeda.Services.DeliveryZone.Services;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;
using CloudSync;
using System.Xml.Linq;
using MyAndromeda.CloudSynchronization.Services;


namespace MyAndromeda.Web.Controllers.Api
{
    public class DeliveryZonesByRadiusController : ApiController
    {
        private readonly IMyAndromedaLogger loggger;
        private readonly ICurrentSite currentSite;
        private readonly INotifier notifier;
        private readonly IMyAndromedaLogger logger;
        private readonly IDeliveryZoneService deliveryZoneService;
        private readonly ISynchronizationTaskService synchronizationTaskService;
        private readonly WorkContextWrapper workContextWrapper;

        public DeliveryZonesByRadiusController(IMyAndromedaLogger loggger,
            INotifier notifier,
            ICurrentSite currentSite,
            IDeliveryZoneService deliveryZoneService,
            ISynchronizationTaskService synchronizationTaskService,
            WorkContextWrapper workContextWrapper,
            IMyAndromedaLogger logger)
        {
            this.logger = logger;
            this.loggger = loggger;
            this.notifier = notifier;
            this.currentSite = currentSite;
            this.deliveryZoneService = deliveryZoneService;
            this.synchronizationTaskService = synchronizationTaskService;
            this.workContextWrapper = workContextWrapper;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/DeliveryZonesByRadius/GeneratePostCodeSectors")]
        public async Task<IEnumerable<string>> GeneratePostCodeSectors(DeliveryZoneNameViewModel model)
        {
            var postcodes = this.GeneratePostcodeSectors(model.OriginPostCode, model.RadiusCovered)
                .Select(e => e.Trim())
                .OrderBy(e => e)
                .Distinct();

            //var response = Request.CreateResponse(HttpStatusCode.OK, new[] { postcodes });

            return postcodes;
        }


        [HttpGet]
        [Route("api/{AndromedaSiteId}/DeliveryZonesByRadius/Read")]
        public DeliveryZoneNameViewModel Read()
        {
            var store = this.currentSite.Store;

            var model = deliveryZoneService.GetDeliveryZonesByRadius(store.Id);
            var viewModel = model.ToViewModel();

            if (model == null)
            {
                viewModel.OriginPostCode = store.Address.PostCode;
                viewModel.RadiusCovered = 5;
            }

            return viewModel;
        }

        [HttpPost]
        [Route("api/{AndromedaSiteId}/DeliveryZonesByRadius/Update")]
        public async Task<HttpResponseMessage> Update(DeliveryZoneNameViewModel viewModel)
        {
            bool success = true;
            DeliveryZoneNameViewModel result = null;

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(error => error.ErrorMessage);

                return Request.CreateResponse(HttpStatusCode.BadRequest, errors);
            }

            try
            {
                if (viewModel == null) { throw new NullReferenceException("viewModel is null. Not data was posted"); }

                var dbModel = this.deliveryZoneService.GetDeliveryZonesByRadius(this.currentSite.SiteId) ?? new DeliveryZoneName();

                //update the database model(s) from the view model
                viewModel.StoreId = currentSite.SiteId;
                viewModel.CreateOrUpdateDataModel(dbModel);

                //save changes
                this.deliveryZoneService.SaveDeliveryZones(dbModel);
                this.notifier.Notify("The Delivery Zone changes have been saved.");

                result = dbModel.ToViewModel();

                this.synchronizationTaskService.CreateTask(new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.CloudSynchronizationTask()
                {
                    Completed = false,
                    Description = "Site Postcodes have been updated",
                    Name = "Postcodes Update",
                    Timestamp = DateTime.UtcNow,
                    StoreId = this.workContextWrapper.Current.CurrentSite.AndromediaSiteId,
                    InvokedByUserId = this.workContextWrapper.Current.CurrentUser.User.Id,
                    InvokedByUserName = this.workContextWrapper.Current.CurrentUser.User.Username
                });
            }
            catch (Exception e)
            {
                this.loggger.Error("Delivery zones could not be saved", e);
                this.notifier.Error("Delivery Zones had errors while saving.");
                success = false;
            }

            var response = Request.CreateResponse(success ? HttpStatusCode.OK : HttpStatusCode.ExpectationFailed, result);

            return response;
        }

        public List<string> GeneratePostcodeSectors(string originPostcode, decimal? radius)
        {
            List<string> list = new List<string>();
            //originPostcode = "AB10 1";
            //radius = 3;
            string url = Convert.ToString(ConfigurationSettings.AppSettings["PostCodeService"]);
            string outputXml = string.Empty;

            if (!string.IsNullOrEmpty(url))
            {

                originPostcode = originPostcode.Replace(" ", string.Empty);
                url = string.Format(url + "?postcode={0}&radius={1}", originPostcode, radius);


                this.logger.Debug("User {0} is querying new postcodes for: {1}", workContextWrapper.Current.CurrentUser.User.Username, url);

                HttpHelper.RestGet(url, out outputXml);

                if (!string.IsNullOrEmpty(outputXml))
                {
                    XElement xElement = XElement.Parse(outputXml);
                    IEnumerable<XElement> xElements = xElement.Elements();
                    var postcodes = xElements.Where(w => w.Name.LocalName.Equals("PostcodeList", StringComparison.CurrentCultureIgnoreCase)).FirstOrDefault();
                    list = postcodes.Elements().Select(ele => ele.Value).ToList();
                }
            }

            return list;
        }

    }
}