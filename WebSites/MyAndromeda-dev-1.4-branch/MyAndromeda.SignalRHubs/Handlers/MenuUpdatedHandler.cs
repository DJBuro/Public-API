using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using MyAndromeda.Menus.Events;
using MyAndromeda.Core.Authorization;

namespace MyAndromeda.SignalRHubs.Handlers
{
    public class MenuUpdatedHandler : IMenuItemChangedEvent
    {
        public void UpdatedMenuItems(EditMenuItemsContext context)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.StoreHub>();

            hubContext.Clients.Group(hubContext
                .GetStoreGroup(context.Site.AndromediaSiteId))
                .updateLocalItems(context);
        }
        public void UpdatedToppings(EditToppingItemsContext context)
        {
            var hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.StoreHub>();

            hubContext.Clients.Group(hubContext
                .GetStoreGroup(context.Site.AndromediaSiteId))
                .updateToppings(context);
        }
    }
}
