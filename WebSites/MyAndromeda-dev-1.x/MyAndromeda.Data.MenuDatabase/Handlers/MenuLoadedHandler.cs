using System;
using System.Linq;
using MyAndromeda.Data.AcsServices.Events;
using MyAndromeda.Data.MenuDatabase.Services;
using System.Diagnostics;
using MyAndromeda.Data.AcsServices.Models;
using System.Collections.Generic;

namespace MyAndromeda.Data.MenuDatabase.Handlers
{
    public class MenuLoadedHandler : IMenuLoadingEvent
    {
        private readonly IQuickMenuDataService quickMenuService;

        private readonly IAccessDbMenuVersionDataService accessMenuVersionService;
        private readonly IAccessMenuItemDataService accessMenuItemDataService;
        private readonly IAccessPriceDataService accessPriceDataService;
        private readonly IAccessMenuItemWebOrderingService webOrderingService;
        private readonly IAccessSecondaryCategoryDataService secondaryCategoryService;
        private readonly IAccessGroupDataService groupDataService;

        public MenuLoadedHandler(
            IQuickMenuDataService quickMenuService,
            IAccessDbMenuVersionDataService accessMenuVersionService,
            IAccessMenuItemDataService accessMenuItemDataService,
            IAccessPriceDataService accessPriceDataService,
            IAccessMenuItemWebOrderingService webOrderingService,
            IAccessSecondaryCategoryDataService secondaryCategoryService,
            IAccessGroupDataService groupDataService) 
        {
            this.quickMenuService = quickMenuService;
            this.groupDataService = groupDataService;
            this.secondaryCategoryService = secondaryCategoryService;
            this.webOrderingService = webOrderingService;
            this.accessPriceDataService = accessPriceDataService;
            this.accessMenuItemDataService = accessMenuItemDataService;
            this.accessMenuVersionService = accessMenuVersionService;
        }
        
        public int Order
        {
            get
            {
                return 1;
            }
        }

        const string Name = "Adding details from the Menu.mdb";
        public string HandlerName
        {
            get
            {
                return Name;
            }
        }

        public void Loaded(IMenuLoadingContext context)
        {
            if (!this.accessMenuItemDataService.AndromedaSiteId.HasValue) 
            {
                this.accessMenuItemDataService.SetupWithAndromedaSiteId(context.AndromedaSiteId);
            }
            if (!this.webOrderingService.AndromedaSiteId.HasValue) 
            {
                this.webOrderingService.SetupWithAndromedaSiteId(context.AndromedaSiteId);
            }
            if (!accessPriceDataService.AndromedaSiteId.HasValue) 
            {
                this.accessPriceDataService.SetupWithAndromedaSiteId(context.AndromedaSiteId);
            }

            //check over the access db menu for more 'stuff'
            if (accessMenuVersionService.IsAvailable(context.AndromedaSiteId))
            {
                Trace.WriteLine("updating asc menu with access database");
                this.FillMenuFromAccessDbMenuItems(context.Menu);

                Trace.WriteLine("updated menu items - access database");
                Trace.WriteLine("Updating menu items - web sequence");
                this.webOrderingService.CreateDefaultOrder(context.Menu.MenuItems);

                Trace.WriteLine("updated menu items - web sequence");
                //this.accessMenuItemService.UpdateMenuItems(context.Menu, context.Menu.MenuItems);
                //flag for the view to enable / disable editing extra detail
                context.Menu.DetailEditable = true;
            }
        }

        private void FillMenuFromAccessDbMenuItems(MyAndromedaMenu menu)
        {
            var accessMenu = quickMenuService.List(e => true).GroupBy(e=> e.nUID).ToDictionary(e=> e.Key, e=> e.ToList());

            foreach (var menuItem in menu.MenuItems)
            {
                if (!accessMenu.ContainsKey(menuItem.Id)) { continue; }
                //i want it to throw up if there is more 
                var menuRow = accessMenu[menuItem.Id].First();
                    //this.accessMenuItemDataService.List(e => e.nUID == menuItem.Id).SingleOrDefault();
                
                if (menuRow != null)
                {
                    menuItem.Name = menuRow.Value(e => e.strItemName, string.Empty);
                    menuItem.WebName = menuRow.Value(e => e.WebName, string.Empty);
                    menuItem.WebDescription = menuRow.Value(e => e.WebDescription, string.Empty);
                    menuItem.WebSequence = menuRow.Value(e => e.WebSequence, 0);

                   // var prices = this.accessPriceDataService.GetPrices(menuItem.Id);
                    menuItem.Prices = new MyAndromedaMenuItemPrices()
                    {
                        Collection = (decimal)menuRow.Value("n_product price", 0) / 100,
                        Delivery = (decimal)menuRow.Value(e=> e.n_TakeAwayPrice, 0) / 100,
                        Instore = (decimal)menuRow.Value(e=> e.n_EatinPrice, 0) / 100
                    };
                    //menuItem.Price = ((decimal)menuRow.n_PricesRow.nPrice) / 100;
                }
            }

            Dictionary<int, Category> categoryList = new Dictionary<int, Category>();
            Dictionary<int, Group> groupList = new Dictionary<int, Group>();

            foreach (var toppingItem in menu.Toppings)
            {
                var menuRow = this.accessMenuItemDataService.List(e => e.nUID == toppingItem.Id).FirstOrDefault();

                if (menuRow != null)
                {
                    toppingItem.Name = menuRow.Value(e => e.strItemName, string.Empty);
                    var prices = this.accessPriceDataService.GetPrices(toppingItem.Id);

                    if (!categoryList.ContainsKey(menuRow.nSubCat))
                    {
                        var accessSecondaryGroup = this.secondaryCategoryService.List(e => e.nUID == menuRow.nSubCat).FirstOrDefault();
                        categoryList.Add(menuRow.nSubCat, accessSecondaryGroup.ToModel());
                    }

                    if (!groupList.ContainsKey(menuRow.nGroupCat))
                    {
                        var accessGroup = this.groupDataService.List(e => e.nUID == menuRow.nGroupCat).FirstOrDefault();
                        groupList.Add(menuRow.nGroupCat, accessGroup.ToModel());
                    }


                    toppingItem.CollectionPrice = prices.Collection;
                    toppingItem.DeliveryPrice = prices.Delivery;
                    toppingItem.DineInPrice = prices.InStore;
                    toppingItem.SubCategory = categoryList.ContainsKey(menuRow.nSubCat) ? categoryList[menuRow.nSubCat] : null;
                    toppingItem.Group = groupList.ContainsKey(menuRow.nGroupCat) ? groupList[menuRow.nGroupCat] : null;
                }
            }

        }
    }

    public static class Extensions 
    {
        public static Category ToModel(this MyAndromeda.Data.MenuDatabase.Models.Database.Menu.n_SecondaryCatRow secondaryCategoryRow) 
        {
            return new Category()
            {
                Id = secondaryCategoryRow.Value(e => e.nUID, 0),
                Name = secondaryCategoryRow.Value(e => e.strName, string.Empty)
            };
            
        }

        public static Group ToModel(this MyAndromeda.Data.MenuDatabase.Models.Database.Menu.n_GroupsRow groupRow) 
        {
            return new Group()
            {
                Id = groupRow.Value(e => e.nUID, 0),
                Name = groupRow.Value(e => e.strName, string.Empty)
            };
        }
    }
}
