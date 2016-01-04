using MyAndromeda.Data.AcsServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Menus.Events;
using MyAndromeda.Core;

namespace MyAndromeda.Menus.Services.Data
{
    public interface IToppingItemService : IDependency
    {
        /// <summary>
        /// Updates the access database menu items.
        /// </summary>
        /// <param name="clientMenuItems">The client menu items.</param>
        void UpdateAccessDatabaseMenuItems(IEnumerable<MyAndromedaTopping> clientMenuItems);
    }

    public class UpdateToppingItemService : IToppingItemService
    {
        private readonly IAccessMenuDataSetService accessMenuDataService;
        private readonly IWorkContext workContext;
        private readonly IMenuItemChangedEvent[] menuItemEventHandlers; 

        public UpdateToppingItemService(IAccessMenuDataSetService accessMenuDataService, IWorkContext workContext, IMenuItemChangedEvent[] menuItemEventHandlers)
        {
            this.menuItemEventHandlers = menuItemEventHandlers;
            this.workContext = workContext;
            this.accessMenuDataService = accessMenuDataService;
        }

        public void UpdateAccessDatabaseMenuItems(IEnumerable<MyAndromedaTopping> clientMenuItems)
        {
            foreach (var item in clientMenuItems) 
            {
                //to work around the odd bug where it doesnt like editing more than this amount in one go before saving. 
                var tempCollection = new[] { item };
                var pairs = tempCollection.Select(clientItem => new
                {
                    AccessDbItem = this.accessMenuDataService.List(row => tempCollection.Any(id => id.Id == row.nUID)).First(),
                    Item = clientItem
                }).ToArray();

                foreach (var pair in pairs) 
                {
                    var accessDbRow = pair.AccessDbItem;
                    var clientMenuItem = pair.Item;

                    accessDbRow.strItemName = string.IsNullOrWhiteSpace(clientMenuItem.Name) ? accessDbRow.strItemName : clientMenuItem.Name;

                    var prices = new
                    {
                        Delivery = Convert.ToInt32(clientMenuItem.DeliveryPrice * 100),
                        Collection = Convert.ToInt32(clientMenuItem.CollectionPrice * 100),
                        InStore = Convert.ToInt32(clientMenuItem.DineInPrice * 100)
                    };

                    this.accessMenuDataService.UpdatePrice(accessDbRow.nUID, prices.InStore, prices.Delivery, prices.Collection);
                }

                this.accessMenuDataService.SaveChanges();

            }

            var user = this.workContext.CurrentUser.User;
            var site = this.workContext.CurrentSite.Site;

            var eventMessage = new EditToppingItemsContext(user, site, clientMenuItems, "Toppings");

            foreach (var changeEventHandler in menuItemEventHandlers)
            {
                changeEventHandler.UpdatedToppings(eventMessage);
            }
        }
    }
}
