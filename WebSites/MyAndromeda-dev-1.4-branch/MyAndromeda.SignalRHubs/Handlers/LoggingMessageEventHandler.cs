using Microsoft.AspNet.SignalR;
using MyAndromeda.Core.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging.Events;
using System;
using System.Linq;
using System.Web.Mvc;

namespace MyAndromeda.SignalRHubs.Handlers
{
    public class LoggingMessageEventHandler : IMyAndromedaLoggingMessageEvent
    {
        private readonly IApplicationSettings applicationSettings;
        private readonly IHubContext hubContext;

        public LoggingMessageEventHandler(IApplicationSettings applicationSettings) 
        {
            
            this.applicationSettings = applicationSettings;

            this.hubContext = GlobalHost.ConnectionManager.GetHubContext<Hubs.StoreHub>();
        }

        private string ExtendMessage(string message) 
        {
            try
            {
                var currentUser = DependencyResolver.Current.GetService<ICurrentUser>();

                if (!currentUser.Available) { return message; }

                return string.Format("{0} - {1}", currentUser.User.Username, message);
            }
            catch (Exception)
            {
                return message;
            }
        }

        public void OnDebug(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .OnDebug(this.ExtendMessage(message));
        }

        public void OnInfo(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .onInfo(this.ExtendMessage(message));
        }

        public void OnTrace(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .onTrace(this.ExtendMessage(message));
        }

        public void OnWarn(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .onWarn(this.ExtendMessage(message));
        }

        public void OnError(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .OnError(this.ExtendMessage(message));
        }

        public void OnError(Exception exception)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .OnError(this.ExtendMessage(exception.Message));
        }

        public void OnFatal(string message)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .onTrace(this.ExtendMessage(message));
        }

        public void OnFatal(Exception exception)
        {
            if (!applicationSettings.SignalrAsALogger) { return; }

            this.hubContext.Clients.Group(hubContext.GetRoleGroup(ExpectedUserRoles.SuperAdministrator))
                .onFatal(this.ExtendMessage(exception.Message));
        }
    }
}
