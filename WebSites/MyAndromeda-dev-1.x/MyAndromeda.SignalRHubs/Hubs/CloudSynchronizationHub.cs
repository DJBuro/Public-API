using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MyAndromeda.CloudSynchronization.Services;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Logging;

namespace MyAndromeda.SignalRHubs.Hubs
{
    [HubName("cloudSynchronization")]
    [MyAndromedaAuthorizeAttribute]
    public class CloudSynchronizationHub : Hub
    {
        private readonly IAuthorizer authorizer;
        private readonly IMyAndromedaLogger logger;

        private readonly ICurrentUser user;
        private readonly ICurrentSite site;

        private readonly ISynchronizationTaskService dbService;

        public CloudSynchronizationHub(
            ICurrentUser user,
            ICurrentSite site,
            ISynchronizationTaskService dbService,
            IAuthorizer authorizer,
            IMyAndromedaLogger logger)
        { 
            this.authorizer = authorizer;
            this.logger = logger;
            this.user = user;
            this.site = site;
            this.dbService = dbService;
        }

        public int GetTasksToRun() 
        {
            //if (!this.authorizer.Authorize(PermissionsSyncTask.ViewSyncTaskLogging))
            //{
            //    throw new NotAuthorizedException();
            //}
            var data = this.dbService.GetTasksToRun(DateTime.UtcNow);

            return data.Count();
        }

        public override Task OnConnected()
        {
            this.logger.Info("Cloud Sync Hub Connected");

            try
            {
                this.logger.Info("Join hub things");
                this.MyAndromedaJoinGroups(this.user, this.site);

                this.Clients.Caller.ping(string.Format("Cloud sync hub ping : {0}", DateTime.UtcNow));
                this.logger.Info("client pinged");
            
                var count = this.GetTasksToRun();
                this.Clients.Caller.tasks(count);
                this.logger.Info("all done - connected");
            
            }
            catch (Exception e) 
            {
                this.logger.Error(e);
            }

            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            this.MyAndromedaReJoin(this.user, this.site);

            return base.OnReconnected();
        }

        //public override Task OnDisconnected()
        //{
        //    this.MyAndromedaLeave(this.user);

        //    return base.OnDisconnected();
        //}
    }
}
