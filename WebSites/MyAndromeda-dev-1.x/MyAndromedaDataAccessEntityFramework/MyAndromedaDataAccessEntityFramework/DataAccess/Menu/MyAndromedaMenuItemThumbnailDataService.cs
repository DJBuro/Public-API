using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.Entity;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccess.Domain.Menus.Items;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public class MyAndromedaMenuItemThumbnailDataService : IMyAndromedaMenuItemThumbnailDataService
    {
        private readonly MyAndromedaDbContext dbContext;

        public MyAndromedaMenuItemThumbnailDataService(MyAndromedaDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        /// <summary>
        /// Gets the menu items.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        public IQueryable<MenuItemThumbnail> GetMenuItemThumbnails(int andromedaSiteId)
        {
            var table = this.dbContext.MenuItemThumbnails;
            var query = table
                             .Where(e => e.MenuItems.Any(menuItem => menuItem.SiteMenu.AndromediaId == andromedaSiteId));

            return query;
        }

        /// <summary>
        /// Clears the thumbnails for menu item.
        /// </summary>
        /// <param name="menuItem"></param>
        public void ClearThumbnailsForItem(MenuItem menuItem)
        {
            var table = this.dbContext.MenuItems;

            //fetch menu item
            //enforce the expectation that there are one/no records for the table 
            var dbMenutItem = table
                                   .Where(e => e.Id == menuItem.Id)
                                   .SingleOrDefault();

            //no item excellent.
            if (dbMenutItem == null)
            {
                return;
            }

            var thumbs = dbMenutItem.MenuItemThumbnails;
            thumbs.Clear();

            this.dbContext.SaveChanges();
        }

        /// <summary>
        /// Clears the thumbnails for items.
        /// </summary>
        /// <param name="menuItems">The menu items.</param>
        public void ClearThumbnailsForItems(IEnumerable<MenuItem> menuItems)
        {
            foreach (var menuItem in menuItems)
            {
                this.ClearThumbnailsForItem(menuItem);
            }
        }

        /// <summary>
        /// Adds the thumbnail for menu item.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <param name="thumb">The thumb.</param>
        /// <returns></returns>
        public MenuItemThumbnail AddThumbnailForMenuItem(MenuItem menuItem, ThumbnailImageDomainModel thumb)
        {
            return this.AddThumbnailForMenuItems(new[] { menuItem }, thumb);
        }

        /// <summary>
        /// Adds the thumbnail for menu items.
        /// </summary>
        /// <param name="menuItems">The menu items.</param>
        /// <param name="thumb">The thumb.</param>
        /// <returns></returns>
        public MenuItemThumbnail AddThumbnailForMenuItems(IEnumerable<MenuItem> menuItems, ThumbnailImageDomainModel thumb)
        {
            var table = this.dbContext.MenuItemThumbnails;
            var entity = table.Create();

            entity.Id = Guid.NewGuid();
            entity.Src = thumb.Url ?? string.Empty;
            entity.Alt = thumb.Alt ?? string.Empty;
            entity.FileName = thumb.FileName;
            entity.Height = thumb.Height;
            entity.Width = thumb.Width;

            table.Add(entity);

            this.dbContext.SaveChanges();
            //associate with the menu-item
            var menuItemIds = menuItems.Select(e => e.Id).ToArray();
            var menuItemEntitys = this.dbContext.MenuItems.Where(e => menuItemIds.Any(id => id == e.Id)).ToArray();

            foreach (var menuItemEntity in menuItemEntitys)
            {
                menuItemEntity.MenuItemThumbnails.Add(entity);
            }

            this.dbContext.SaveChanges();

            return entity;
        }

        public void UpdateMenuHasChanged(Guid menuId)
        {
            var table = this.dbContext.SiteMenus;
            var query = table.Where(e => e.Id == menuId);

            var menu = query.Single();
            if (menu == null)
            {
                return;
            }

            menu.LastUpdatedUtc = DateTime.UtcNow;
            this.dbContext.SaveChanges();
        }

        public MenuItem GetOrCreateMenuItem(SiteMenu menu, int itemId)
        {
            var table = this.dbContext.MenuItems;
            var query = table
                             .Where(e => e.SiteMenu.Id == menu.Id)
                             .Where(e => e.ItemId == itemId);
            //.Where(e => e.SiteMenus.Any(itemMenu => itemMenu.Id == menu.Id));

            var result = query.SingleOrDefault();

            if (result == null)
            {
                result = this.CreateMenuItem(menu, itemId);
                table.Add(result);
                this.dbContext.SaveChanges();
            }

            return result;
        }

        public void UpdateMenuItems(SiteMenu siteMenu, MenuItem[] dbItems)
        {
            var ids = dbItems.Select(e => e.ItemId);
            var dbMenuItems = GetOrCreateMenuItems(siteMenu, ids).ToArray();

            foreach (var dbEntity in dbMenuItems)
            {
                var updateWith = dbItems.FirstOrDefault(e => e.ItemId == dbEntity.ItemId);
                dbEntity.Name = updateWith.Name;
                dbEntity.WebName = updateWith.WebName;
                dbEntity.WebDescription = updateWith.WebDescription;
                dbEntity.Enabled = updateWith.Enabled;
            }

            this.dbContext.SaveChanges();
        }

        public void UpdateMenuItem(SiteMenu siteMenu, MenuItem menuItem)
        {
            var table = this.dbContext.MenuItems;
            var query = table.Where(e => e.Id == menuItem.Id);
            var result = query.Single();

            result.Name = menuItem.Name;
            result.WebName = menuItem.WebName;
            result.WebDescription = menuItem.WebDescription;

            this.dbContext.SaveChanges();
        }

        public IEnumerable<MenuItem> GetOrCreateMenuItems(SiteMenu menu, IEnumerable<int> similarMenuItemIds)
        {
            List<MenuItem> menuItemsToReturn = new List<MenuItem>();
            //menuItemsToReturn = this.GetOrCreateMenuItems(menu, similarMenuItemIds);

            var menuItemTable = this.dbContext.MenuItems;
            var menuItemQuery = menuItemTable
                .Where(e => e.SiteMenu.Id == menu.Id)
                .Select(e => new
                {
                    ItemId = e.ItemId,
                    Item = e,
                    Thumbs = e.MenuItemThumbnails
                });

            var allMenuItemResult = menuItemQuery.ToArray();

            List<MenuItem> menuItemsToCreate = new List<MenuItem>();

            foreach (var id in similarMenuItemIds)
            {
                //there should never be more than one, curious that dev1 is doing that. 
                var items = allMenuItemResult.Where(e => e.ItemId == id);
                var itemResult = items.FirstOrDefault(e => e.ItemId == id);

                MenuItem item = null;
                if (itemResult == null)
                {
                    item = this.CreateMenuItem(menu, id);
                    menuItemsToCreate.Add(item);
                }

                menuItemsToReturn.Add(item ?? itemResult.Item);
            }

            foreach (var item in menuItemsToCreate)
            {
                dbContext.MenuItems.Add(item);
            }

            this.dbContext.SaveChanges();
            
            return menuItemsToReturn;
        }

        public async Task<IEnumerable<MenuItem>> GetOrCreateMenuItemsAsync(SiteMenu menu, IEnumerable<int> similarMenuItemIds)
        {

            List<MenuItem> menuItemsToReturn = new List<MenuItem>();
            //menuItemsToReturn = this.GetOrCreateMenuItems(menu, similarMenuItemIds);

            var menuItemTable = this.dbContext.MenuItems;
            var menuItemQuery = menuItemTable
                .Include(e=> e.MenuItemThumbnails)
                .Where(e => e.SiteMenu.Id == menu.Id)
                .Select(e => new
                {
                    ItemId = e.ItemId,
                    Item = e,
                    Thumbs = e.MenuItemThumbnails
                });

            var allMenuItemResult = await menuItemQuery.ToArrayAsync();

            List<MenuItem> menuItemsToCreate = new List<MenuItem>();

            foreach (var id in similarMenuItemIds)
            {
                //there should never be more than one, curious that dev1 is doing that. 
                var items = allMenuItemResult.Where(e => e.ItemId == id);
                var itemResult = items.FirstOrDefault(e => e.ItemId == id);

                MenuItem item = null;
                if (itemResult == null)
                {
                    item = this.CreateMenuItem(menu, id);
                    menuItemsToCreate.Add(item);
                }

                menuItemsToReturn.Add(item ?? itemResult.Item);
            }

            foreach (var item in menuItemsToCreate)
            {
                dbContext.MenuItems.Add(item);
            }

            await this.dbContext.SaveChangesAsync();

            return menuItemsToReturn;
        }

        private MenuItem CreateMenuItem(SiteMenu menu, int itemId)
        {
            var entity = new MenuItem();// table.Create();

            entity.Id = Guid.NewGuid();
            entity.ItemId = itemId;
            entity.SiteMenuId = menu.Id;
            entity.Enabled = true;

            return entity;
        }
        //public void GetMenuAndTranslate(int andromedaSiteId, Action<MenuDbJob> workWithSiteMenuWhileOpen)
        //{
        //    //using (var dbConext = NewContext())
        //    {
        //        var menu = this.siteMenuDataService.GetMenu(andromedaSiteId); //this.GetMenuWithContext(dbConext, andromedaSiteId);
        //        var menuItemQuery = dbConext.MenuItems.Where(e => e.SiteMenu.AndromediaId == andromedaSiteId);
        //        var menuItemThumbQuery = dbConext.MenuItems
        //            .Where(e=> e.SiteMenu.AndromediaId == andromedaSiteId)
        //            .SelectMany(e=> e.MenuItemThumbnails)
        //            //dbConext.MenuItemThumbnails
        //            //    .Where(e => e.MenuItemThumbnailRelations
        //            //                .Any(menuItemLink => menuItemLink.MenuItem.SiteMenu.AndromediaId == andromedaSiteId));
        //        var menuItemThumbnailLinkQuery = dbConext.MenuItemThumbnailsLinkTable
        //                                                 .Where(e => e.MenuItem.SiteMenu.AndromediaId == andromedaSiteId);
        //        var job = new MenuDbJob(menu, menuItemQuery, menuItemThumbnailLinkQuery, menuItemThumbQuery);
        //        workWithSiteMenuWhileOpen(job);
        //    }
        //}
    }
}