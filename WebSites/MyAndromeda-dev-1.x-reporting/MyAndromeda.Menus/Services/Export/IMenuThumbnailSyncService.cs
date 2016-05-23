using MyAndromeda.Core;
using System;
using System.Linq;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Logging;
using MyAndromeda.Storage;
using Newtonsoft.Json;
using MyAndromeda.Data.DataAccess.Sites;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Data.Domain;
using System.Xml;

namespace MyAndromeda.Menus.Services.Export
{
    public interface IMenuThumbnailSyncService : ITransientDependency
    {
        void CreateDataStructureForJsonAndXml(int andromediaSiteId);
    }

    public class MenuThumbnailSyncService : IMenuThumbnailSyncService
    {
        private readonly IMyAndromedaLogger logger;
        private readonly IMyAndromedaSiteMenuDataService menuDataService;

        private readonly IMyAndromedaMenuItemDataService menuItemDataService;
        private readonly IMyAndromedaMenuItemThumbnailDataService menuItemThumbnailDataService;
        private readonly IMyAndromedaMenuSyncDataService menuSyncSerivce;
        private readonly IMyAndromedaSiteMediaServerDataService menuMediaServerService;
        private readonly IStorageService storage;
        private readonly ISiteDataService siteDataService;

        public MenuThumbnailSyncService(IMyAndromedaLogger logger,
            IMyAndromedaSiteMenuDataService menuDataService,
            IMyAndromedaMenuItemThumbnailDataService menuItemThumbnailDataService,
            IMyAndromedaMenuSyncDataService menuSyncSerivce,
            IMyAndromedaSiteMediaServerDataService menuMediaServerService,
            IStorageService storage,
            ISiteDataService siteDataService,
            IMyAndromedaMenuItemDataService menuItemDataService)
        {
            this.menuItemDataService = menuItemDataService;
            this.siteDataService = siteDataService;
            this.storage = storage;
            this.menuMediaServerService = menuMediaServerService;
            this.menuSyncSerivce = menuSyncSerivce;
            this.menuItemThumbnailDataService = menuItemThumbnailDataService;
            this.logger = logger;
            this.menuDataService = menuDataService;
        }

        public void CreateDataStructureForJsonAndXml(int andromediaSiteId)
        {
            logger.Debug("begin converting to json and xml menu - andromeiaSiteId: {0}", andromediaSiteId);
            //output object
            object conversion = null;

            SiteMenuMediaServer mediaServer = this.menuMediaServerService.GetMediaServerWithDefault(andromediaSiteId);
            SiteDomainModel store = siteDataService
                .List(e => e.AndromedaSiteId == andromediaSiteId)
                .SingleOrDefault();
            
            
            SiteMenu menu = this.menuDataService.GetMenu(andromediaSiteId);
            logger.Debug("Translate menu: {0} (last updated: {1} {2})", menu.Id, menu.LastUpdatedUtc.GetValueOrDefault().ToShortDateString(), menu.LastUpdatedUtc.GetValueOrDefault().ToShortTimeString());

            IQueryable<MenuItemThumbnail> thumbnails = this.menuItemThumbnailDataService.GetMenuItemThumbnails(menu.AndromediaId);
            
            var thumbnailList = thumbnails.Select(e => new {
                ItemIds = e.MenuItems.Select(menuItem => menuItem.ItemId),
                //e.Id,
                e.Src,
                e.Width,
                e.Height
            }).ToArray();

            var disabledItems = this.menuItemDataService.Query(andromediaSiteId)
                .Where(e => !e.Enabled)
                .Select(e => new { Id = e.ItemId }).ToArray();
            var output = new
            {
                Server = new
                {
                    Endpoint = this.storage.ContentPath(mediaServer.Address, mediaServer.ContentPath, store.ExternalSiteId)
                },
                MenuItemThumbnails = thumbnailList,
                DisabledItems = disabledItems
                //thumbnailGroup.Keys.Select(e =>
                //{
                //    var items = thumbnailGroup[e];
                //    var first = items.First();
                //    return new
                //    {
                //        Src = first.Src,
                //        Height = first.Height,
                //        Width = first.Width,
                //        ItemIds = items.Select(item => item.MenuItemId).ToArray()
                //    };
                //}).ToArray()
            };

            conversion = output;
   

            logger.Debug("Create the json from: {0}", menu.Id);
            
            string json = JsonConvert.SerializeObject(conversion);

            logger.Debug("create the xml from: {0}", menu.Id);
            
            XmlDocument xml = JsonConvert.DeserializeXmlNode(json, "MenuThumbnails");
            string xmlValue = xml.OuterXml;

            logger.Debug(message: "Storing the sync data");

            this.menuSyncSerivce.SyncMenuThumbnails(andromediaSiteId, xmlValue, json);
        }
    }
}
