using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAndromeda.Data.AcsServices.Events;
using MyAndromeda.Data.AcsServices.Models;
using MyAndromedaDataAccessEntityFramework.DataAccess.Menu;

namespace MyAndromeda.Menus.Handlers
{
    public class MenuLoadedHandler : IMenuLoadingEvent
    {
        private readonly IMyAndromedaMenuItemDataService menutItemDataService;

        public MenuLoadedHandler(IMyAndromedaMenuItemDataService menutItemDataService) 
        {
            this.menutItemDataService = menutItemDataService;
        }

        public int Order
        {
            get { return 100; }
        }

        
        //const string Name = "I cant remember what this was going to do";
        const string Name = "This adds the enabled flag from the database ... lucky thing.";


        public string HandlerName
        {
            get
            {
                return Name;
            }
        }

        public void Loaded(IMenuLoadingContext context)
        {
            // TODO: Implement this method

            var menuItems = this.menutItemDataService.Query(context.AndromedaSiteId).ToArray();

            foreach (var item in context.Menu.MenuItems) 
            {
                var menuItem = menuItems.FirstOrDefault(e => e.ItemId == item.Id);

                if (menuItem == null) 
                {
                    item.Enabled = true;

                    continue;
                }

                item.Enabled = menuItem.Enabled;
            }
        }

    }
}
