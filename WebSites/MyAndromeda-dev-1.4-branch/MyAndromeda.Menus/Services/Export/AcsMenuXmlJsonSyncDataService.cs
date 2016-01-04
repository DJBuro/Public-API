using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Logging;
using MyAndromeda.Menus.Services.Data;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using Newtonsoft.Json;

namespace MyAndromeda.Menus.Services.Export
{
    public class AcsMenuXmlJsonSyncDataService : IAcsMenuXmlJsonSyncDataService
    {
        private readonly IStoreDataService storeDataService; 
        private readonly IGetAcsAddressesService getAcsAddressesService;
        private readonly IAcsMenuServiceAsync acsMenuServiceService;
        private readonly IMyAndromedaLogger logger;
        private readonly IMyAndromedaMenuSyncDataService menuSyncSerivce;
        private readonly IAccessDbMenuVersionDataService dbMenuVersionDataService;

        public AcsMenuXmlJsonSyncDataService(IAcsMenuServiceAsync acsMenuServiceService, IMyAndromedaLogger logger, IGetAcsAddressesService getAcsAddressesService, IStoreDataService storeDataService, IMyAndromedaMenuSyncDataService menuSyncSerivce, IAccessDbMenuVersionDataService dbMenuVersionDataService) 
        {
            this.dbMenuVersionDataService = dbMenuVersionDataService;
            this.menuSyncSerivce = menuSyncSerivce;
            this.storeDataService = storeDataService;
            this.getAcsAddressesService = getAcsAddressesService;
            this.logger = logger;
            this.acsMenuServiceService = acsMenuServiceService;
        }

        public async Task UpdateAcsMenuAsync(int andromedaSiteId)
        {
            //this will not update the child sites ... 
            if (!dbMenuVersionDataService.IsAvailable(andromedaSiteId)) { return; }

            //otherwise do all this stuff.
            var store = this.storeDataService.Get(e => e.AndromedaSiteId == andromedaSiteId);
            var endpoints = await this.getAcsAddressesService.GetMenuEndpointsAsync(store);

            var currentMenu = await acsMenuServiceService.GetMenuDataFromEndpointsAsync(andromedaSiteId, store.ExternalId, endpoints);
            dynamic onlineMenu = await acsMenuServiceService.GetRawMenuDataFromEndpointsAsync(andromedaSiteId, endpoints);

            var itemListDictionary = GetItemList(onlineMenu);
            var toppingListDictionary = GetToppingList(onlineMenu);

            logger.Debug("Updating ACS menu item values for: {0})", andromedaSiteId);
            this.UpdateItems(
                onlineMenu: onlineMenu,
                itemDictionary: itemListDictionary,
                mergedAndUpdatedMenu: currentMenu);

            logger.Debug("Updating ACS menu topping values for: {0})", andromedaSiteId);
            this.UpdateToppings(
                toppingDictionary: toppingListDictionary,
                mergedAndUpdatedMenu: currentMenu);

            var json = JsonConvert.SerializeObject(onlineMenu);
            
            this.menuSyncSerivce.SyncActualMenu(andromedaSiteId, string.Empty, json, 0); 
        }

        private IDictionary<int, dynamic> GetItemList(dynamic onlineMenu) 
        {
            Dictionary<int, dynamic> itemList = new Dictionary<int, dynamic>();

            foreach (dynamic item in onlineMenu.Items) 
            {
                try
                {
                    int id = item.Id;

                    if (itemList.ContainsKey(id)) 
                    {
                        logger.Error("Ignoring item! This menu item should not be here twice:" + id);
                        
                        continue;
                    }

                    itemList.Add(id, item);
                }
                catch (Exception e) 
                {
                    logger.Error(e);
                }
            }

            return itemList;
        }

        private IDictionary<int, dynamic> GetToppingList(dynamic onlineMenu) 
        {
            Dictionary<int, dynamic> itemList = new Dictionary<int, dynamic>();

            foreach (var item in onlineMenu.Toppings) 
            {
                int id = item.Id;

                if (itemList.ContainsKey(id))
                {
                    logger.Error("Ignoring topping! This menu item should not be here twice:" + id);
                    continue;
                }

                itemList.Add(id, item);
            }

            return itemList;
        }

        private void UpdateToppings(IDictionary<int, dynamic> toppingDictionary, MyAndromedaMenu mergedAndUpdatedMenu)
        {
            foreach (var menuTopping in mergedAndUpdatedMenu.Toppings) 
            {
                if (!toppingDictionary.ContainsKey(menuTopping.Id))
                {
                    continue;
                }

                var item = toppingDictionary[menuTopping.Id];
                
                //update the json
                item.Name = menuTopping.Name;
                item.DelPrice = Convert.ToInt32(menuTopping.DeliveryPrice.GetValueOrDefault());
                item.ColPrice = Convert.ToInt32(menuTopping.CollectionPrice.GetValueOrDefault());
            }
        }

        private void UpdateItems(dynamic onlineMenu, IDictionary<int, dynamic> itemDictionary, MyAndromedaMenu mergedAndUpdatedMenu)
        {
            //JArray
            dynamic itemNames = onlineMenu.ItemNames;

            foreach (var menuItem in mergedAndUpdatedMenu.MenuItems) 
            {
                if (!itemDictionary.ContainsKey(menuItem.Id))
                {
                    continue;
                }
                
                var item = itemDictionary[menuItem.Id];
                ////update name and description 
                
                //index is stupid for item names
                int itemNameIndex = item.Name;
                itemNames[itemNameIndex] = string.IsNullOrWhiteSpace(menuItem.WebName) ? menuItem.Name : 
                                           menuItem.WebName;

                //item description
                item.Desc = menuItem.WebDescription;

                ////update prices
                if (menuItem.Prices == null)
                {
                    continue;
                }

                //delivery, collection, dine in
                item.DelPrice = menuItem.Prices.Delivery * 100;
                item.ColPrice = menuItem.Prices.Collection * 100;
                item.DineInPrice = menuItem.Prices.Instore * 100;

                //item web sequence
                item.DispOrder = menuItem.WebSequence; //this isn't updating ? 
            }
        }
    }
}