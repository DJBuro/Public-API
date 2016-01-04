using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Kendo.Mvc.UI;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Logging;
using MyAndromeda.Services.DeliveryZone.Services;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using MyAndromeda.Web.Areas.DeliveryZone.Model;
using MyAndromeda.CloudSynchronization.Services;
using CloudSync;
using System.Configuration;
using System.Xml;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using MyAndromeda.Web.Areas.DeliveryZone.Models;


namespace MyAndromeda.Web.Areas.DeliveryZone.Controllers
{
    public class DeliveryZoneController : Controller
    {

        /* Utility services */
        private readonly IAuthorizer authorizer;
        private readonly IMyAndromedaLogger logger;

        private readonly INotifier notifier;
        private readonly ITranslator translator;
        private readonly ISynchronizationTaskService synchronizationTaskService;
        private readonly WorkContextWrapper workContextWrapper;
        private readonly ICurrentSite currentSite;

        /* Context variables */
        private readonly IDeliveryZoneService deliveryZoneService;

        public DeliveryZoneController(
            WorkContextWrapper workContextWrapper,
            IAuthorizer authorizer,
            IMyAndromedaLogger logger,
            INotifier notifier,
            ITranslator translator,
            ISynchronizationTaskService synchronizationTaskService,
            IDeliveryZoneService deliveryZoneService,
            ICurrentSite currentSite
            )
        {
            this.authorizer = authorizer;
            this.logger = logger;
            this.notifier = notifier;
            this.translator = translator;
            this.deliveryZoneService = deliveryZoneService;
            this.workContextWrapper = workContextWrapper;
            this.synchronizationTaskService = synchronizationTaskService;
            this.currentSite = currentSite;
        }


        public ActionResult Index([DataSourceRequest] DataSourceRequest request)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewDeliveryZone)) 
            {
                this.notifier.Notify("You are not allowed to visit the delivery zones");

                return new HttpUnauthorizedResult();
            }

            var dataList = deliveryZoneService.List().Where(e => e.Removed == false).ToList();
            Model.DeliveryAreaViewModel deliveryZone = new Model.DeliveryAreaViewModel();
            if (dataList != null)
            {
                dataList.ToViewModel(deliveryZone);
                return View(deliveryZone);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(Model.DeliveryAreaViewModel deliveryZone)
        {
            if (!this.authorizer.Authorize(UserPermissions.EditDeliveryZone))
            {
                this.notifier.Notify("You are not allowed to edit the delivery zones");

                return new HttpUnauthorizedResult();
            }

            deliveryZone.DeliveryAreasList = new List<string>();
            if (deliveryZone.DeliveryAreas != null)
            {
                deliveryZone.DeliveryAreasList = deliveryZone.DeliveryAreas.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries)
                   .ToArray()
                   .Select(s => s.Trim())
                   .Where(s => (!string.IsNullOrEmpty(s.Trim())))
                   .Distinct()
                   .OrderBy(e => e)
                   .ToList();
            }
            deliveryZone.DeliveryAreas = String.Join("\r\n", deliveryZone.DeliveryAreasList).Trim();

            IList<DeliveryArea> existingDeliveryAreas = this.deliveryZoneService.List().Where(e => e.Removed == false).ToList();
            Model.DeliveryAreaViewModel existingDeliveryZone = new DeliveryAreaViewModel();
            existingDeliveryAreas.ToViewModel(existingDeliveryZone);

            if (deliveryZone.DeliveryAreasList.Count == 0)
            {
                bool success = this.deliveryZoneService.Delete(deliveryZone.SiteId);
            }
            else
            {
                var newDeliveryAreas = deliveryZone.DeliveryAreasList.Except(existingDeliveryZone.DeliveryAreasList).ToList();
                var removedDelieryAreas = existingDeliveryZone.DeliveryAreasList.Except(deliveryZone.DeliveryAreasList).ToList();
                if (removedDelieryAreas.Count() > 0 || newDeliveryAreas.Count() > 0)
                {
                    //get data version
                    int version = MyAndromedaDataAccessEntityFramework.Model.DataVersionHelper.GetNextDataVersion(new AndroAdminDbContext());

                    foreach (var item in newDeliveryAreas)
                    {
                        this.deliveryZoneService.Create(new DeliveryArea { DeliveryArea1 = item, Removed = false, StoreId = currentSite.SiteId, Id = Guid.NewGuid(), DataVersion = version });
                    }

                    foreach (var item in removedDelieryAreas)
                    {
                        this.deliveryZoneService.Delete(new DeliveryArea { DeliveryArea1 = item, StoreId = currentSite.SiteId, DataVersion = version });
                    }
                }
            }

            this.synchronizationTaskService.CreateTask(new MyAndromedaDataAccessEntityFramework.Model.MyAndromeda.CloudSynchronizationTask()
            {
                Completed = false,
                Description = "Site Delivery areas have been updated",
                Name = "DeliveryArea Update",
                Timestamp = DateTime.UtcNow,
                StoreId = this.workContextWrapper.Current.CurrentSite.AndromediaSiteId,
                InvokedByUserId = this.workContextWrapper.Current.CurrentUser.User.Id,
                InvokedByUserName = this.workContextWrapper.Current.CurrentUser.User.Username
            });


            return View(deliveryZone);

        }

        [HttpGet]
        public ActionResult DeliveryZonesByRadius()
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewDeliveryZone))
            {
                this.notifier.Notify("You are not allowed to visit the delivery zones");

                return new HttpUnauthorizedResult();
            }

            int storeId = currentSite.SiteId;
            DeliveryZoneNameViewModel model = new DeliveryZoneNameViewModel { PostCodeSectors = new List<PostCodeSectorViewModel>() };

            DeliveryZoneName deliveryZoneName = deliveryZoneService.GetDeliveryZonesByRadius(storeId);

            deliveryZoneName.ToViewModel();

            return View("DeliveryZonesByRadius", model);
        }

        [HttpPost]
        public ActionResult DeliveryZonesByRadius(DeliveryZoneNameViewModel model)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewDeliveryZone))
            {
                this.notifier.Notify("You are not allowed to edit the delivery zones");

                return new HttpUnauthorizedResult();
            }

            DeliveryZoneName deliveryzone = new DeliveryZoneName();
            if (model != null)
            {
                model.CreateOrUpdateDataModel(deliveryzone);
                deliveryZoneService.SaveDeliveryZones(deliveryzone);
            }

            return View("DeliveryZonesByRadius", model);
        }

        //DeliveryZones by Radius
        [HttpPost]
        public ActionResult GetPostCodeSectors(string originPostcode, decimal? radius)
        {
            if (!this.authorizer.Authorize(UserPermissions.ViewDeliveryZone))
            {
                this.notifier.Notify("You are not allowed to visit the delivery zones");

                return new HttpUnauthorizedResult();
            }

            List<string> postcodes = GeneratePostcodeSectors(originPostcode, radius);
            return Json(postcodes);
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
                url = string.Format(url + "?origin={0}&radius={1}", originPostcode, radius);

                this.logger.Debug("User {0} is querying new postcodes for: {1}", workContextWrapper.Current.CurrentUser.User.Username, url);

                HttpHelper.RestGet(url, out outputXml);
                if (!string.IsNullOrEmpty(outputXml))
                {
                    XDocument doc = XDocument.Parse(outputXml);
                    list = doc.Root.Elements()
                                       .Select(element => element.Value)
                                       .ToList();

                }
            }

            return list;
        }
    }
}