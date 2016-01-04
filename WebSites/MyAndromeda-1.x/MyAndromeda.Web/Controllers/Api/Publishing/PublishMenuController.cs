using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Services.Menu;
using MyAndromeda.WebApiClient.Models;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using System.Threading.Tasks;
using MyAndromeda.Services.Gprs;

namespace MyAndromeda.Web.Controllers.Api.Publishing
{
    public class PublishMenuController : ApiController
    {
        private readonly ITranslator translator;
        private readonly INotifier notifier;
        private readonly IMyAndromedaLogger logger;

        private readonly Func<IPublishingMenuService> publishingMenuServiceFactory;
        private readonly Func<IMyAndromedaSiteMenuDataService> siteMenuDataServiceFactory;
        private readonly IGprsService gprsService;

        private readonly IMenuPublishEvents[] publishEvents;

        public PublishMenuController(INotifier notifier,
            IMyAndromedaLogger logger,
            ITranslator translator,
            IMenuPublishEvents[] publishEvents,
            Func<IPublishingMenuService> publishingMenuServiceFactory,
            Func<IMyAndromedaSiteMenuDataService> siteMenuDataServiceFactory,
            IGprsService gprsService
            )
        {
            this.translator = translator;
            this.gprsService = gprsService;
            this.notifier = notifier;
            this.logger = logger;
            this.publishingMenuServiceFactory = publishingMenuServiceFactory;
            this.siteMenuDataServiceFactory = siteMenuDataServiceFactory;
            this.publishEvents = publishEvents;
        }

        public async Task<HttpResponseMessage> Sync([FromBody]MenuSyncModel model)
        {
            var siteMenuDataService = this.siteMenuDataServiceFactory();
            SiteMenu sitemenu = siteMenuDataService.GetMenu(model.AndromedaSiteId);

            var context = new MenuPublishContext()
            {
                Cancel = false,
                AndromedaSiteId = model.AndromedaSiteId
            };

            var publishingMenuService = this.publishingMenuServiceFactory();

            bool failed = false;

            try
            {

                publishingMenuService.StartedTask(sitemenu);

                foreach (var ev in publishEvents)
                {
                    //if a gprs printer sync will be performed with ACS 
                    await ev.Publishing(context);
                }

                publishingMenuService.PublishThumbnailJsonAndXml(sitemenu);

                foreach (var ev in publishEvents)
                {
                    await ev.Published(context);
                }

                publishingMenuService.CreateSynchronizationTask(sitemenu);

                //if (this.gprsService.IsStoreGprsDevicebyAndromedaSiteId(sitemenu.AndromediaId)) 
                //{
                    //this.notifier.Success(translator.T("Your menu menu is nearly updated."), true);
                //}

                publishingMenuService.CompletedTask(sitemenu);
            }

            catch (Exception e)
            {
                publishingMenuService.FailedTask(sitemenu);
                failed = true;

                this.notifier.Error(translator.T("There was a error recreating your menu for online use. The service will try again shortly."));

                this.logger.Error("Error processing the publish menu task");
                this.logger.Error(e);
            }

            return failed
                ? new HttpResponseMessage(HttpStatusCode.InternalServerError)
                : new HttpResponseMessage(HttpStatusCode.OK);
        }

    }
}
