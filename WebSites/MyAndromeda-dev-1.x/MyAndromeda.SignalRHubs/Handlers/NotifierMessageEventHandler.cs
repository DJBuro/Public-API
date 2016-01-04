using System;
using System.Linq;
using Microsoft.AspNet.SignalR;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Framework.Notification;

namespace MyAndromeda.SignalRHubs.Handlers
{
    /// <summary>
    /// Hub connections to the users of a store / user - to notify workflow success/errors/notifications.
    /// </summary>
    public class NotifierMessageEventHandler : INotifierEvent
    {
        private readonly IWorkContext workContext;
        private readonly IHubContext hubContext;

        public NotifierMessageEventHandler(IWorkContext workContext)
        {
            this.workContext = workContext;
            this.hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.StoreHub>();
        }

        public void OnError(NotificationContext message)
        {
            message.UserName = workContext.CurrentUser.Available
                ? workContext.CurrentUser.User.Username
                : "MyAndromeda";

            var storeGroup = hubContext.GetStoreGroup(workContext.CurrentSite.AndromediaSiteId);

            if (workContext.CurrentUser.Available)
            {
                var connectionsToShoutAt = hubContext.GetUserConnections(workContext.CurrentUser.User.Id).ToArray();

                //shout at self.
                hubContext.Clients.Clients(connectionsToShoutAt).onNotifierError(message);

                //shout at others roaming the store.
                if (message.NotifyOthersLoggedIntoStore)
                {
                    //notify everyone but user. 
                    hubContext.Clients
                        .Group(storeGroup, connectionsToShoutAt)
                        .onNotifierError(message);
                }

                return;
            }

            if (!message.NotifyOthersLoggedIntoStore)
                return;

            //group 
            hubContext.Clients
                .Group(storeGroup)
                .onNotifierError(message);
        }

        public void OnDebug(NotificationContext message)
        {
            message.UserName = workContext.CurrentUser.Available
                ? workContext.CurrentUser.User.Username
                : "MyAndromeda";

            var storeGroup = hubContext.GetStoreGroup(workContext.CurrentSite.AndromediaSiteId);

            if (workContext.CurrentUser.Available)
            {
                var connectionsToShoutAt = hubContext.GetUserConnections(workContext.CurrentUser.User.Id).ToArray();

                //notify users.
                hubContext.Clients.Clients(connectionsToShoutAt).onNotifierDebug(message);

                if (message.NotifyOthersLoggedIntoStore)
                {
                    //notify everyone but user. 
                    hubContext.Clients
                        .Group(storeGroup, connectionsToShoutAt)
                        .onNotifierDebug(message);
                }

                return;
            }

            if (!message.NotifyOthersLoggedIntoStore)
                return;

            //group - notify user
            hubContext.Clients
                .Group(storeGroup)
                .onNotifierDebug(message);
        }

        public void OnNotify(NotificationContext message)
        {
            if (string.IsNullOrWhiteSpace(message.UserName)) { 
                message.UserName = workContext.CurrentUser.Available
                    ? workContext.CurrentUser.User.Username
                    : "MyAndromeda";
            }

            var storeGroup = hubContext.GetStoreGroup(workContext.CurrentSite.AndromediaSiteId);

            if (workContext.CurrentUser.Available)
            {
                var connectionsToShoutAt = hubContext.GetUserConnections(workContext.CurrentUser.User.Id).ToArray();

                //notify users.
                hubContext.Clients.Clients(connectionsToShoutAt).onNotifierNotify(message);

                if (message.NotifyOthersLoggedIntoStore)
                {
                    //notify everyone but user. 
                    hubContext.Clients
                        .Group(storeGroup, connectionsToShoutAt)
                        .onNotifierNotify(message);
                }

                return;
            }

            if (!message.NotifyOthersLoggedIntoStore)
                return;

            //group - notify user
            hubContext.Clients
                .Group(storeGroup)
                .onNotifierNotify(message);
        }

        public void OnSuccess(NotificationContext message)
        {
            message.UserName = workContext.CurrentUser.Available
                ? workContext.CurrentUser.User.Username
                : "MyAndromeda";

            var storeGroup = hubContext.GetStoreGroup(workContext.CurrentSite.AndromediaSiteId);

            if (workContext.CurrentUser.Available)
            {
                var connectionsToShoutAt = hubContext.GetUserConnections(workContext.CurrentUser.User.Id).ToArray();

                //notify users.
                hubContext.Clients.Clients(connectionsToShoutAt).onNotifierSuccess(message);

                if (message.NotifyOthersLoggedIntoStore)
                {
                    //notify everyone but user. 
                    hubContext.Clients
                        .Group(storeGroup, connectionsToShoutAt)
                        .onNotifierSuccess(message);
                }

                return;
            }

            if (!message.NotifyOthersLoggedIntoStore)
                return;

            //group - notify user
            hubContext.Clients
                .Group(storeGroup)
                .onNotifierSuccess(message);


        }
    }
}