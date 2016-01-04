using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CloudSync;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Framework.Translation;
using MyAndromeda.Menus.Services.Menu;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Services.Export;
using MyAndromeda.WebApiClient.Syncing;

namespace MyAndromeda.Web.Areas.Menu.Controllers
{
    public class MenuSyncController : Controller
    {
        private readonly IWorkContext workContext;
        
        private readonly IMyAndromedaLogger logger;
        private readonly ITranslator translator;
        private readonly INotifier notifer;
        
        
        private readonly IFtpMenuManagerService menuManagerService;
        private readonly IPublishingMenuService publishingThumbnailMenuService;
        private readonly ISyncWebCallerController syncWebCallerController;


        public MenuSyncController(IWorkContext workContext, IMyAndromedaLogger logger, ITranslator translator, INotifier notifer, IFtpMenuManagerService menuManagerService, 
            IPublishingMenuService publishingMenuService,
            ISyncWebCallerController syncWebCallerController)
        {
            this.syncWebCallerController = syncWebCallerController;
            this.logger = logger;
            this.translator = translator;

            this.publishingThumbnailMenuService = publishingMenuService;
            this.notifer = notifer;
            this.menuManagerService = menuManagerService;
            this.workContext = workContext;
        }

        public async Task<ActionResult> Publish(bool? menu, bool? thumbnails, DateTime? dateUtc) 
        {
            var andromedaId = this.workContext.CurrentSite.AndromediaSiteId;
            var storeMenu = this.menuManagerService.GetMenu(andromedaId);
            var userName = this.workContext.CurrentUser.User.Username;

            if (!menu.GetValueOrDefault() && !thumbnails.GetValueOrDefault())
            {
                throw new ArgumentNullException("menu and thumbnails are null");
            }

            this.publishingThumbnailMenuService.AddHistoryLog(storeMenu, userName,
                menu.GetValueOrDefault() && thumbnails.GetValueOrDefault(),
                menu.GetValueOrDefault(),
                thumbnails.GetValueOrDefault(),
                dateUtc
            );

            //doesn't really matter if it is future or now... is managed on rameses 
            if (menu.GetValueOrDefault())
            {
                //however i don't want to wait for it. 
                this.menuManagerService.CreatePublishFtpTask(storeMenu, dateUtc);
            }

            //publish later
            if (dateUtc.GetValueOrDefault() > DateTime.UtcNow)
            {
                
                if (menu.GetValueOrDefault() && thumbnails.GetValueOrDefault())
                {
                    this.notifer.Notify(translator.T("The menu and thumbnails are getting ready... Please wait.", dateUtc.GetValueOrDefault().ToLongDateString()), true);
                }
                else if (menu.GetValueOrDefault())
                {
                    this.notifer.Notify(translator.T("The menu has been queued to publish on {0}.", dateUtc.GetValueOrDefault().ToLongDateString()), true);
                }
                else if (thumbnails.GetValueOrDefault())
                {
                    this.notifer.Notify(translator.T("The thumbnails have been queued to publish on {0}.", dateUtc.GetValueOrDefault().ToLongDateString()), true);
                }

                if (thumbnails.GetValueOrDefault()) { 
                    //future publish 
                    publishingThumbnailMenuService.PublishMenuLater(storeMenu, dateUtc.GetValueOrDefault(DateTime.UtcNow));
                }
            }
            //publish now.
            else 
            {
                if(menu.GetValueOrDefault() && thumbnails.GetValueOrDefault())
                {
                    this.notifer.Notify(translator.T("The menu and thumbnails are being prepared."), true);
                }
                else if (menu.GetValueOrDefault()) 
                {
                    this.notifer.Notify(translator.T("The menu data is now being prepared."), true);
                }
                else if (thumbnails.GetValueOrDefault())
                {
                    this.notifer.Notify(translator.T("The menu thumbnails are now being prepared."), true);
                }

                if(thumbnails.GetValueOrDefault())
                {
                    if (await syncWebCallerController.RequestMenuSyncAsync(andromedaId))
                    {
                        this.notifer.Notify(translator.T("The online menu is cooked, and getting ready to deliver.", true));
                    }
                    else 
                    {
                        this.notifer.Error(translator.T("Creating the online menu failed.", true));
                    }
                    //publishingThumbnailMenuService.PublishNow(storeMenu);
                }
            }

            ////doesn't really matter if it is future or now... is managed on rameses 
            //if (menu.GetValueOrDefault())
            //{
            //    //however i don't want to wait for it. 
            //    this.menuManagerService.CreatePublishFtpTask(storeMenu, dateUtc);
            //}

            return Json(new { 
                HaveMenu = menu.GetValueOrDefault(),
                HaveThumbs = thumbnails.GetValueOrDefault(),
                Date = dateUtc.GetValueOrDefault()
            });
        }
        
    //    public ActionResult Sync()
    //    {
    //        var andromedaId = this.workContextWrapper.Current.CurrentSite.AndromediaSiteId;
    //        var storeMenu = this.menuManagerService.GetMenu(andromedaId);
    //        /* if the site is running with the access database as well */
    //        if (this.accessDbMenuVersionService.IsAvailable(andromedaId)) 
    //        {
               
    //            var accessDatabaseVersion = this.accessDbMenuVersionService.IncrementVersion(andromedaId);

    //            /* update the database what version it is */
    //            storeMenu.AccessMenuVersion = accessDatabaseVersion;
    //            this.menuManagerService.SetVersion(storeMenu);

    //            /* quickly tell the ftp job that the menu wants chucking up... and to publish on a date */
    //            this.menuManagerService.AskToPublishFtp(storeMenu, null);
    //            /* hold back the menu data to be published on a date */ 
                
    //        }

    //        try
    //        {
    //            /*
    //             * Process: 
    //             * 1. Compile json and xml based on MyAndromeda menu structure and data
    //             * 2. Queue sync task for cloud servers 
    //             * 3. Finish
    //             */

    //            this.logger.Debug("Save the complete thumbnail menu to andro admin db");
    //            //save menu changes to andro-admin
    //            //todo:
    //            //this.menuManagerService.AskToPublishThumbnailData(storeMenu, null);
    //            this.menuSyncService.CreateDataStructureForJsonAndXml(this.workContextWrapper.Current.CurrentSite.Site.AndromediaSiteId);

    //            this.logger.Debug("Begin Syncing");

    //            // Push the change out to the ACS (cloud) servers
    //            string errorMessage = string.Empty;
    //            errorMessage = SyncHelper.ServerSync();

    //            // Success
    //            if (errorMessage.Length == 0)
    //            {
    //                this.notifer.Notify("1. Thumbnail changes have been successfully updated");
    //                this.notifer.Notify("2. Changes to the menu have been sent off.");
    //            }
    //            else
    //            {
    //                this.notifer.Error("Failed to update cloud servers");
    //                this.notifer.Exception(new Exception(errorMessage));
    //            }
    //        }
    //        catch (Exception e)
    //        {
    //            this.notifer.Error("There was an error saving the state of the menu");

    //            throw;
    //        }

    //        return RedirectToAction("Index", "MenuNavigation");
    //    }
    }
}