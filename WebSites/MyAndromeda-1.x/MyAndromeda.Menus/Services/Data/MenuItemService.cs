using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.AcsServices.Context;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Menus.Events;
using MyAndromedaDataAccess.Domain.Menus.Items;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;

namespace MyAndromeda.Menus.Services.Data
{
    public class MenuItemService : IMenuItemService 
    {
        private readonly IMyAndromedaMenuItemDataService myAndromedaMenuDataService;
        private readonly IActiveMenuContext menuContext;
        private readonly IAccessDbMenuVersionDataService accessDbVersion;
        private readonly IAccessMenuDataSetService accessMenuDataService;
        private readonly IMenuItemChangedEvent[] menuItemEventHandlers; 

        public MenuItemService(
            WorkContextWrapper workContext,
            IMyAndromedaMenuItemDataService myAndromedaMenuDataService,
            IActiveMenuContext menuContext,
            IAccessDbMenuVersionDataService accessDbVersion,
            IAccessMenuDataSetService accessMenuDataService,
            IMenuItemChangedEvent[] events)
        {
            this.accessMenuDataService = accessMenuDataService;
            this.menuItemEventHandlers = events;
            this.WorkContext = workContext;
            this.accessDbVersion = accessDbVersion;
            this.menuContext = menuContext;
            this.myAndromedaMenuDataService = myAndromedaMenuDataService;
        }

        /// <summary>
        /// Gets or sets the work context.
        /// </summary>
        /// <value>The work context.</value>
        public WorkContextWrapper WorkContext { get; private set; }

        public void UpdateMenuItemsBatch(SiteMenu menu, IEnumerable<MyAndromedaMenuItem> clientMenuItems, UpdateSection section) 
        {
            var dbItems = this.myAndromedaMenuDataService.GetOrCreateMenuItems(menu, clientMenuItems.Select(e => e.Id)).ToArray();

            foreach (var dbItem in dbItems) 
            {
                //var clientMenuItem = clientMenuItems.Single(e => e.Id == dbItem.ItemId);
                var clientMenuItem = clientMenuItems.FirstOrDefault(e => e.Id == dbItem.ItemId);
                
                //update fields - web name, description 
                dbItem.Update(clientMenuItem);
            }
            
            //update the database 
            this.myAndromedaMenuDataService.UpdateMenuItems(menu, dbItems);

            //update access database?
            if (accessDbVersion.IsAvailable(menu.AndromediaId))
            {
                //some reason it does not update correctly in batch.
                //and only updates the one item.
                foreach (var mi in clientMenuItems) 
                {
                    this.UpdateAccessDatabaseMenuItems(new[] { mi });
                }
                //this.UpdateAccessDatabaseMenuItems(clientMenuItems);
            }

            if (section == UpdateSection.Data)
            { 
                //update the thumbnails
                var thumbnailedItems = clientMenuItems.Where(e => e.Thumbs != null && e.Thumbs.Any()).ToArray();
                
                //Remove thumbs - Need to be deleted
                foreach (var menuItem in clientMenuItems)
                {
                    //if (menuItem.Thumbs == null || menuItem.Thumbs.Count() == 0)
                    //{
                        var dbMenuItemWithThumbs = dbItems.Where(e => e.ItemId == menuItem.Id).ToArray();
                        this.ClearThumbnailsForItems(dbMenuItemWithThumbs.Select(e => e));
                    //}
                }

                //Common thumbs - need to be updated 
                var dbThumbnailedItems = dbItems
                                                .Where(e => thumbnailedItems.Any(c => c.Id == e.ItemId))
                                                .Select(e => new
                                                {
                                                    dbItem = e,
                                                    clientItem = thumbnailedItems.Single(c => c.Id == e.ItemId)
                                                })
                                                .ToArray();
                
                //this.ClearThumbnailsForItems(dbThumbnailedItems.Select(e => e.dbItem));
                foreach (var pair in dbThumbnailedItems) 
                {
                    this.UpdateMenuItemThumbnails(new[] { pair.dbItem }, pair.clientItem.Thumbs);   
                }
            }

            //notify others of updates
            var user = this.WorkContext.Current.CurrentUser.User;
            var site = this.WorkContext.Current.CurrentSite.Site;

            foreach (var changeEventHandler in menuItemEventHandlers)
            {
                var eventMessage = new EditMenuItemsContext(user, site, clientMenuItems, section.ToString());

                changeEventHandler.UpdatedMenuItems(eventMessage);
            }
        }

        public void UpdateMenuItems(SiteMenu menu, IEnumerable<MyAndromedaMenuItem> items)
        {
            System.Diagnostics.Trace.WriteLine("Fetch menu items");
            var dbItems = this.GetMenuItems(menu, items).ToArray();

            foreach (var item in items) 
            {
                var dbMenuItem = dbItems.FirstOrDefault(e => e.ItemId == item.Id);
                //update additional fields
                dbMenuItem.Update(item);
            }

            System.Diagnostics.Trace.WriteLine("start Updating menu items");
            this.myAndromedaMenuDataService.UpdateMenuItems(menu, dbItems);
            System.Diagnostics.Trace.WriteLine("completed Updating menu items");
            
            if (accessDbVersion.IsAvailable(menu.AndromediaId)) 
            {
                System.Diagnostics.Trace.WriteLine("start Updating access menu items");
                this.UpdateAccessDatabaseMenuItems(items);
                System.Diagnostics.Trace.WriteLine("completed updating access menu items");
            }

            //update the flag that the menu has changed.
            this.UpdateRecordThatMenuHasChanged(menu.Id);
        }

        private MenuItem[] GetMenuItems(SiteMenu menu, IEnumerable<MyAndromedaMenuItem> items)
        {
            var dbItems = this.myAndromedaMenuDataService.GetOrCreateMenuItems(menu, items.Select(e => e.Id));

            return dbItems.ToArray();
        }
        
        public void UpdateMenuItemThumbnailsOnly(SiteMenu menu, IEnumerable<MyAndromedaMenuItem> clientMenuItems, IEnumerable<ThumbnailImage> thumbs)
        {
            var affectedDbMenuItems = Enumerable.Empty<MenuItem>();

            affectedDbMenuItems = this.GetSimilarItems(menu, clientMenuItems.Select(e => e.Id).ToArray());

            this.UpdateThumbnailPart(affectedDbMenuItems, thumbs);

            //update the flag that the menu has changed.
            this.UpdateRecordThatMenuHasChanged(menu.Id);
        }

        private void UpdateThumbnailPart(IEnumerable<MenuItem> affectedDbMenuItems, IEnumerable<ThumbnailImage> thumbs) 
        {
            //clear relations
            this.ClearThumbnailsForItems(affectedDbMenuItems);

            //remake each relation
            this.UpdateMenuItemThumbnails(affectedDbMenuItems, thumbs);
        }

        private void UpdateMenuItemThumbnails(IEnumerable<MenuItem> menuItems, IEnumerable<ThumbnailImage> thumbs) 
        {
            foreach (var thumb in thumbs)
            {
                var fullUrl = thumb.Url;
                var localUrl = thumb.Url.Replace(this.menuContext.ContentPath, string.Empty);

                thumb.Url = localUrl;
                var entity = this.AddThumbnailToMenuItems(menuItems, thumb);

                //update the model - which in turn notifies that it has changed client side
                thumb.Id = entity.Id;
                thumb.Url = fullUrl;
            }
        }

        private void UpdateAccessDatabaseMenuItems(IEnumerable<MyAndromedaMenuItem> clientMenuItems)
        {
            var pairs = clientMenuItems.Select(clientItem => new
            {
                Access = this.accessMenuDataService.List(row => clientMenuItems.Any(id => id.Id == row.nUID)).First(),
                Item = clientItem
            }).ToArray();

            foreach (var pair in pairs) 
            {
                var accessDbRow = pair.Access;
                var clientMenuItem = pair.Item;

                accessDbRow.WebName = string.IsNullOrWhiteSpace(clientMenuItem.WebName) ? string.Empty : clientMenuItem.WebName;
                accessDbRow.WebDescription = string.IsNullOrWhiteSpace(clientMenuItem.WebDescription) ? string.Empty : clientMenuItem.WebDescription;
                accessDbRow.WebSequence = clientMenuItem.WebSequence >= 0 ? clientMenuItem.WebSequence : accessDbRow.WebSequence;

                var prices = new
                {
                    Delivery = Convert.ToInt32(clientMenuItem.Prices.Delivery * 100),
                    Collection = Convert.ToInt32(clientMenuItem.Prices.Collection * 100),
                    InStore = Convert.ToInt32(clientMenuItem.Prices.Instore * 100)
                };

                this.accessMenuDataService.UpdatePrice(accessDbRow.nUID, prices.InStore, prices.Delivery, prices.Collection);
            }

            this.accessMenuDataService.SaveChanges();
        }

        public MenuItem GetMenuItem(SiteMenu menu, int id)
        {
            var dbMenuItem = this.myAndromedaMenuDataService.GetOrCreateMenuItem(menu, id);

            return dbMenuItem;
        }

        public IEnumerable<MenuItem> GetSimilarItems(SiteMenu menu, int[] ids) 
        {
            var dbMenuItems = this.myAndromedaMenuDataService.GetOrCreateMenuItems(menu, ids).ToArray();

            return dbMenuItems;
        }

        /// <summary>
        /// Updates the row that the menu has changed.
        /// </summary>
        /// <param name="guid"></param>
        public void UpdateRecordThatMenuHasChanged(Guid id)
        {
            this.myAndromedaMenuDataService.UpdateMenuHasChanged(id);
        }

        /// <summary>
        /// Clears the thumbnails for items.
        /// </summary>
        /// <param name="menutItems"></param>
        public void ClearThumbnailsForItems(IEnumerable<MenuItem> menutItems)
        {
            this.myAndromedaMenuDataService.ClearThumbnailsForItems(menutItems);
        }

        /// <summary>
        /// Adds the thumbnail to menu items.
        /// </summary>
        /// <param name="attachToMenuItems">The attach to menu items.</param>
        /// <param name="thumbnail">The thumbnail.</param>
        /// <returns></returns>
        public MenuItemThumbnail AddThumbnailToMenuItems(IEnumerable<MenuItem> attachToMenuItems, ThumbnailImage thumbnail)
        {
            return this.myAndromedaMenuDataService.AddThumbnailForMenuItems(attachToMenuItems, thumbnail);
        }
    }
}