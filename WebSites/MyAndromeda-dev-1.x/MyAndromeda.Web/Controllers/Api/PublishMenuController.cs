using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Events;
using MyAndromeda.Menus.Services.Menu;
using MyAndromeda.WebApiClient.Models;
using System.Threading.Tasks;
using MyAndromeda.Data.Model.MyAndromeda;

namespace MyAndromeda.Web.Controllers.Api
{
    public class PublishMenuController : ApiController
    {
        private readonly ITranslator translator;
        private readonly INotifier notifier;
        private readonly IAuthorizer authorizer;
        private readonly IMyAndromedaLogger logger;

        private readonly IPublishingMenuService publishingMenuServiceFactory;
        private readonly IMyAndromedaSiteMenuDataService siteMenuDataServiceFactory;
        private readonly IMenuPublishEvents[] publishEvents;

        public PublishMenuController(INotifier notifier, IMyAndromedaLogger logger, ITranslator translator, IMenuPublishEvents[] publishEvents, IPublishingMenuService publishingMenuServiceFactory, IMyAndromedaSiteMenuDataService siteMenuDataServiceFactory, IAuthorizer authorizer)
        {
            this.authorizer = authorizer;
            this.translator = translator;
            this.notifier = notifier;
            this.logger = logger;
            this.publishingMenuServiceFactory = publishingMenuServiceFactory;
            this.siteMenuDataServiceFactory = siteMenuDataServiceFactory;
            this.publishEvents = publishEvents;
        }

        [HttpPost]
        public async Task<HttpResponseMessage> Sync([FromBody]MenuSyncModel model)
        {
            this.logger.Debug("Menu Sync API Service");

            //if (!this.authorizer.Authorize(MyAndromeda.Web.Areas.Menu.UserPermissions.EditStoreMenu))
            //{
            //    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
            //}

            SiteMenu sitemenu = siteMenuDataServiceFactory.GetMenu(model.AndromedaSiteId);

            var context = new MenuPublishContext()
            {
                Cancel = false,
                AndromedaSiteId = model.AndromedaSiteId
            };


            bool failed = false;

            try
            {

                publishingMenuServiceFactory.StartedTask(sitemenu);

                foreach (var ev in publishEvents)
                {
                    //if a gprs printer sync will be performed with ACS 
                    await ev.Publishing(context);
                }

                publishingMenuServiceFactory.PublishThumbnailJsonAndXml(sitemenu);

                foreach (var ev in publishEvents)
                {
                    await ev.Published(context);
                }

                publishingMenuServiceFactory.CreateSynchronizationTask(sitemenu);

                //if (this.gprsService.IsStoreGprsDevicebyAndromedaSiteId(sitemenu.AndromediaId)) 
                //{
                    //this.notifier.Success(translator.T("Your menu menu is nearly updated."), true);
                //}

                publishingMenuServiceFactory.CompletedTask(sitemenu);
            }

            catch (Exception e)
            {
                publishingMenuServiceFactory.FailedTask(sitemenu);
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
