using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices.Context;
using MyAndromeda.Data.AcsServices.Events;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Data.AcsServices.Services;
using MyAndromeda.Framework.Notification;
using MyAndromeda.Logging;
using MyAndromedaDataAccess.Domain.Menus.Items;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccess.Domain.Menus.Items;
using Newtonsoft.Json;

namespace MyAndromeda.Data.AcsServices
{
    public class MenuServiceAsync : IAcsMenuServiceAsync
    {
        private readonly IMyAndromedaLogger logger;
        private readonly INotifier notifier; 
        private readonly IMenuLoadingEvent[] events;

        private readonly IMenuWebServiceDataAccess menuWebServiceDataAccess;
        private readonly IMyAndromedaMenuItemDataService myAndromedaMenuDataService;
        private readonly IActiveMenuContext menuContext;
        //context 
        private readonly IMenuLoadingContext menuLoadedContext;

        public MenuServiceAsync(IMenuLoadingEvent[] events,
            IMyAndromedaLogger logger,
            IMenuWebServiceDataAccess menuWebServiceDataAccess,
            IMyAndromedaMenuItemDataService myAndromedaMenuDataService,
            IActiveMenuContext menuContext,
            IMenuLoadingContext menuLoadedContext,
            INotifier notifier)
        {
            this.notifier = notifier;
            this.menuLoadedContext = menuLoadedContext;
            this.events = events.OrderBy(e => e.Order).ToArray();
            this.logger = logger;
            this.menuContext = menuContext;
            this.myAndromedaMenuDataService = myAndromedaMenuDataService;
            this.menuWebServiceDataAccess = menuWebServiceDataAccess;
        }

        public async Task<MyAndromedaMenu> GetMenuDataFromEndpointsAsync(int andromedaSiteId, string externalSiteId, IEnumerable<string> endpoints)
        {
            var siteMenuData = await menuWebServiceDataAccess.FetchFromServiceAsync(endpoints);

            var siteMenu = this.TransformToMenu(andromedaSiteId, externalSiteId, siteMenuData);

            return siteMenu;
        }

        public async Task<dynamic> GetRawMenuDataFromEndpointsAsync(int andromedaSiteId, IEnumerable<string> endpoints)
        {
            var siteMenuData = await menuWebServiceDataAccess.FetchFromServiceAsync(endpoints);

            dynamic o = JsonConvert.DeserializeObject(siteMenuData);

            return o;
        }

        /// <summary>
        /// Transforms to menu.
        /// </summary>
        /// <param name="andromedaSiteId">The andromeda site id.</param>
        /// <param name="jsonResult">The json result.</param>
        /// <returns></returns>
        private MyAndromedaMenu TransformToMenu(int andromedaSiteId, string externalSiteId, string jsonResult)
        {
            var rawMenu = JsonConvert.DeserializeObject<RawMenu>(jsonResult);

            var thumbnailList = this.GetThumbnailList(andromedaSiteId, externalSiteId);
            var effectiveMenu = new MyAndromedaMenu(rawMenu, thumbnailList);
            
            this.menuLoadedContext.AndromedaSiteId = andromedaSiteId;
            this.menuLoadedContext.Menu = effectiveMenu;
            this.menuLoadedContext.LoadedThumbnails = true;
            this.menuLoadedContext.AddedThumbnails = thumbnailList.Count;
            this.menuLoadedContext.LoadedAcsMenu = true;

            foreach (var ev in this.events.OrderBy(e=> e.Order)) 
            {
                string failedMessage = string.Format("{0} - An error occurred loading the menu.", ev.HandlerName);

                try
                {
                    ev.Loaded(menuLoadedContext);
                }
                catch(System.Data.OleDb.OleDbException e)
                {
                    this.logger.Error(e);
                    this.notifier.Error(failedMessage, true);

                    if (e.Message.IndexOf("No value given for one or more required parameters", System.StringComparison.InvariantCultureIgnoreCase) >= 0) 
                    { 
                        this.notifier.Error("Last time this occurred the menu was corrupt. Please check the menu in the desktop menu editor and try uploading to the ftp again.");
                        this.notifier.Notify("Updating thumbnails should still be available. Its only names, descriptions and prices that are unavailable.");
                    }
                }  
                catch (System.Exception e)
                {
                    this.logger.Error(e);
                    this.notifier.Error(failedMessage, true);
                }

            }

            return effectiveMenu;
        }

        /// <summary>
        /// nothing here feels right 
        /// </summary>
        /// <param name="andromedaSiteId"></param>
        /// <returns></returns>
        private Dictionary<int, IList<ThumbnailImage>> GetThumbnailList(int andromedaSiteId, string externalSiteId)
        {
            var thumbnailList = new Dictionary<int, IList<ThumbnailImage>>();

            this.menuContext.Setup(andromedaSiteId, externalSiteId);

            var relatedThumbnails = 
                myAndromedaMenuDataService
                                        .GetMenuItems(andromedaSiteId)
                                        .Select(e => new
                                        {
                                            e.Alt,
                                            e.FileName,
                                            e.Height,
                                            e.Id,
                                            e.Src,
                                            e.Width,
                                            ItemsIs = e.MenuItemThumbnailRelations.Select(link => link.MenuItem.ItemId)
                                        })
                                        .OrderByDescending(e => e.Height)
                                        .ToArray();

            var contentEndpoint = this.menuContext.ContentPath;

            //all theses item ids have images
            var ids = relatedThumbnails.SelectMany(e => e.ItemsIs).Distinct();

            foreach (var id in ids)
            {
                thumbnailList.Add(id, new List<ThumbnailImage>());
            }

            foreach (var thumb in relatedThumbnails)
            {
                var thumbnailImage = new ThumbnailImage()
                {
                    Id = thumb.Id,
                    Alt = thumb.Alt,
                    FileName = thumb.FileName,
                    Url = string.Concat(contentEndpoint, thumb.Src),
                    Width = thumb.Width,
                    Height = thumb.Height
                };

                foreach (var id in thumb.ItemsIs)
                {
                    thumbnailList[id].Add(thumbnailImage);
                }
            }

            return thumbnailList;
        }
    }
}