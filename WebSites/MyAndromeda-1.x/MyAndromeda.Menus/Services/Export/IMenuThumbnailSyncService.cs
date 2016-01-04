using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromeda.Logging;
using MyAndromeda.Storage;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using Newtonsoft.Json;

namespace MyAndromeda.Menus.Services.Export
{
    public interface IMenuThumbnailSyncService : ITransientDependency
    {
        void CreateDataStructureForJsonAndXml(int andromediaSiteId);
    }

    public class MenuThumbnailSyncService : IMenuThumbnailSyncService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IMyAndromedaMenuItemDataService menuDataService;
        private readonly IMyAndromedaMenuSyncDataService menuSyncSerivce;
        private readonly IMyAndromedaSiteMediaServerDataService menuMediaServerService;
        private readonly IStorageService storage;
        private readonly ISiteDataService siteDataService;

        public MenuThumbnailSyncService(IMyAndromedaLogger logger, 
            IMyAndromedaMenuItemDataService menuDataService, 
            IMyAndromedaMenuSyncDataService menuSyncSerivce, 
            IMyAndromedaSiteMediaServerDataService menuMediaServerService, 
            IStorageService storage, 
            ISiteDataService siteDataService)
        {
            this.siteDataService = siteDataService;
            this.storage = storage;
            this.menuMediaServerService = menuMediaServerService;
            this.menuSyncSerivce = menuSyncSerivce;
            this.menuDataService = menuDataService;
            this.logger = logger;
        }

        public void CreateDataStructureForJsonAndXml(int andromediaSiteId)
        {
            logger.Debug("begin converting to json and xml menu - andromeiaSiteId: {0}", andromediaSiteId);


            object conversion = null;
            Guid menuId = new Guid();

            menuDataService.GetMenuAndTranslate(andromediaSiteId, (menuDbJob) =>
            {
                var menu = menuDbJob.Menu;
                var store = siteDataService.List(e => e.AndromedaSiteId == andromediaSiteId).SingleOrDefault();
                var mediaServer = this.menuMediaServerService.GetMediaServerWithDefault(andromediaSiteId);

                menuId = menu.Id;

                logger.Debug("Translate menu: {0} (last updated: {1} {2})", menu.Id, menu.LastUpdatedUtc.GetValueOrDefault().ToShortDateString(), menu.LastUpdatedUtc.GetValueOrDefault().ToShortTimeString());

                var thumbnails = menuDbJob.QueryMenuItemThumbnailLinkTable()
                    .Select(e => new
                    {
                        MenuItemId = e.MenuItem.ItemId,
                        e.MenuItemThumbnailId,
                        e.MenuItemThumbnail.Src,
                        e.MenuItemThumbnail.Width,
                        e.MenuItemThumbnail.Height
                    })
                    .ToList();

                var thumbnailGroup = thumbnails
                    .ToLookup(e => e.MenuItemThumbnailId)
                    .ToDictionary(e => e, e => e.ToList());

                
                var output = new
                {
                    Server = new
                    {
                        Endpoint = this.storage.ContentPath(mediaServer.Address, mediaServer.ContentPath, store.ExternalSiteId)
                    },
                    MenuItemThumbnails = thumbnailGroup.Keys.Select(e =>
                    {
                        var items = thumbnailGroup[e];
                        var first = items.First();
                        return new
                        {
                            Src = first.Src,
                            Height = first.Height,
                            Width = first.Width,
                            ItemIds = items.Select(item => item.MenuItemId).ToArray()
                        };
                    }).ToArray()
                };

                conversion = output;
            });

            logger.Debug("Create the json from: {0}", menuId);
            
            var json = JsonConvert.SerializeObject(conversion);

            logger.Debug("create the xml from: {0}", menuId);
            
            var xml = JsonConvert.DeserializeXmlNode(json, "MenuThumbnails");
            var xmlValue = xml.OuterXml;

            logger.Debug("Storing the sync data");

            this.menuSyncSerivce.SyncMenuThumbnails(andromediaSiteId, xmlValue, json);
        }
    }
}
