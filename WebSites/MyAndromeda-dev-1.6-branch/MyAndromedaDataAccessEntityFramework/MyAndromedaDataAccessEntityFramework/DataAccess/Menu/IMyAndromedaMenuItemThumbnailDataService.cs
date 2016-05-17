using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MyAndromeda.Core;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromedaDataAccess.Domain.Menus.Items;

namespace MyAndromeda.Data.DataAccess.Menu
{
    public interface IMyAndromedaMenuItemThumbnailDataService : IDependency
    {

        /// <summary>
        /// Gets the menu items.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <returns></returns>
        IQueryable<MenuItemThumbnail> GetMenuItemThumbnails(int andromedaSiteId);

        /// <summary>
        /// Adds the thumbnail for menu item.
        /// </summary>
        /// <param name="menuItem">The menu item.</param>
        /// <param name="thumb">The thumb.</param>
        /// <returns></returns>
        MenuItemThumbnail AddThumbnailForMenuItem(MenuItem menuItem, ThumbnailImageDomainModel thumb);

        MenuItemThumbnail AddThumbnailForMenuItems(IEnumerable<MenuItem> menuItems, ThumbnailImageDomainModel thumb);

        /// <summary>
        /// Clears the thumbnails for menu item.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="menuItemId">The menu item id.</param>
        void ClearThumbnailsForItem(MenuItem menuItem);

        /// <summary>
        /// Clears the thumbnails for items.
        /// </summary>
        /// <param name="menuItems">The menu items.</param>
        void ClearThumbnailsForItems(IEnumerable<MenuItem> menuItems);

        /// <summary>
        /// Gets the menu and do some work on the item with the same dbcontext.
        /// </summary>
        /// <param name="andromedaSiteId">The Andromeda site id.</param>
        /// <param name="whileOpen">While the context is open.</param>
        //void GetMenuAndTranslate(int andromedaSiteId, Action<MenuDbJob> whileOpen);
        IEnumerable<MenuItem> GetOrCreateMenuItems(SiteMenu menu, IEnumerable<int> similarMenuItemIds);

        Task<IEnumerable<MenuItem>> GetOrCreateMenuItemsAsync(SiteMenu menu, IEnumerable<int> similarMenuItemIds);

        /// <summary>
        /// Gets the or create menu item.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="acsMenuItemId">The acs menu item id.</param>
        /// <returns></returns>
        MenuItem GetOrCreateMenuItem(SiteMenu menu, int acsMenuItemId);

        /// <summary>
        /// Updates the menu item.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="menuItem">The menu item.</param>
        void UpdateMenuItem(SiteMenu menu, MenuItem menuItem);

        //MenuItem CreateMenuItem(SiteMenu menu, int acsMenuItemId);
        void UpdateMenuHasChanged(Guid menuId);

        void UpdateMenuItems(SiteMenu menu, MenuItem[] dbItems);
    }
    //public class MenuDbJob
    //{
    //    private readonly IQueryable<MenuItem> menuItemQuery;
    //    private readonly IQueryable<MenuItemThumbnail> menuItemThumbQuery;
    //    private readonly SiteMenu menu;
    //    private readonly IQueryable<MenuItemThumbnailsLinkTable> menuItemThumbnailLinkQuery;
    //    public MenuDbJob(
    //        SiteMenu menu,
    //        IQueryable<MenuItem> menuItemQuery,
    //        IQueryable<MenuItemThumbnailsLinkTable> menuItemThumbnailLinkQuery,
    //        IQueryable<MenuItemThumbnail> menuItemThumbQuery)
    //    {
    //        this.menuItemThumbnailLinkQuery = menuItemThumbnailLinkQuery;
    //        this.menu = menu;
    //        this.menuItemQuery = menuItemQuery;
    //        this.menuItemThumbQuery = menuItemThumbQuery;
    //    }
    //    public SiteMenu Menu
    //    {
    //        get
    //        {
    //            return this.menu;
    //        }
    //    }
    //    public IQueryable<MenuItem> QueryMenuItems()
    //    {
    //        return this.menuItemQuery;
    //    }
    //    public IQueryable<MenuItemThumbnail> QueryThumbnailImages()
    //    {
    //        return this.menuItemThumbQuery;
    //    }
    //    public IQueryable<MenuItemThumbnailsLinkTable> QueryMenuItemThumbnailLinkTable()
    //    {
    //        return this.menuItemThumbnailLinkQuery;
    //    }
    //}
}