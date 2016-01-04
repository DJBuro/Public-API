using MyAndromeda.Core;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromedaDataAccess.Domain.Menus.Items;
using MyAndromedaDataAccessEntityFramework.Model.MyAndromeda;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyAndromeda.Menus.Services.Data
{
    public interface IMenuItemService : IDependency
    {
        /// <summary>
        /// Gets the menu item.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        MenuItem GetMenuItem(SiteMenu menu, int id);

        /// <summary>
        /// Gets the similar items.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        IEnumerable<MenuItem> GetSimilarItems(SiteMenu menu, int[] ids);

        /// <summary>
        /// Updates the menu items.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="items">The items.</param>
        //void UpdateMenuItems(SiteMenu menu, IEnumerable<MyAndromedaMenuItem> items);

        /// <summary>
        /// Updates the menu items batch.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="clientMenuItems">The client menu items.</param>
        void UpdateMenuItemsBatch(SiteMenu menu, IEnumerable<MyAndromedaMenuItem> clientMenuItems, UpdateSection section);

        /// <summary>
        /// Updates the menu items.
        /// </summary>
        /// <param name="menu">The menu.</param>
        /// <param name="clientMenuItems">The client menu items.</param>
        /// <param name="thumbs">The thumbs.</param>
        void UpdateMenuItemThumbnailsOnly(SiteMenu menu, IEnumerable<MyAndromedaMenuItem> clientMenuItems, IEnumerable<ThumbnailImage> thumbs);

        /// <summary>
        /// Updates the row that the menu has changed.
        /// </summary>
        /// <param name="id">The id.</param>
        void UpdateRecordThatMenuHasChanged(Guid id);

        /// <summary>
        /// Clears the thumbnails for items.
        /// </summary>
        /// <param name="attachToMenuItems">The attach to menu items.</param>
        void ClearThumbnailsForItems(IEnumerable<MenuItem> attachToMenuItems);

        /// <summary>
        /// Adds the thumbnail to menu items.
        /// </summary>
        /// <param name="attachToMenuItems">The attach to menu items.</param>
        /// <param name="thumb">The thumb.</param>
        /// <returns></returns>
        MenuItemThumbnail AddThumbnailToMenuItems(IEnumerable<MenuItem> attachToMenuItems, ThumbnailImage thumb);
    }
}

