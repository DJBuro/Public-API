using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using MyAndromeda.Data.MenuDatabase.Services;
using MyAndromeda.Framework.Authorization;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Web.Areas.Menu.ViewModels;
using MyAndromeda.Logging;

namespace MyAndromeda.SignalRHubs.Hubs
{
    [HubName("storeHub")]
    [MyAndromedaAuthorizeAttribute]
    public class StoreHub : Hub 
    {
        private readonly IAccessDbMenuVersionDataService menuVersionDataService;
        private readonly ICurrentSite site;
        private readonly IAuthorizer authorizer;
        private readonly ICurrentUser user;
        private readonly IMyAndromedaLogger logger;
        private readonly string groupName;

        public StoreHub(ICurrentSite site,
            ICurrentUser user,
            IAuthorizer authorizer,
            IAccessDbMenuVersionDataService menuVersionDataService,
            IMyAndromedaLogger logger)
        { 
            this.logger = logger;
            this.menuVersionDataService = menuVersionDataService;
            this.site = site;
            this.authorizer = authorizer;
            this.user = user;
            
            this.groupName = this.site.Available ? this.site.AndromediaSiteId.ToString() : null;
        }
        
        [HubMethodName("getStoreMenuVersion")]
        public DebugDatabaseMenuViewModel GetStoreMenuVersion() 
        {
            if (!this.site.Available) 
            {
                throw new MissingMemberException("SiteId");
            }

            this.logger.Debug(string.Format("{0} : getStoreMenuVersion started", this.user.User.Username)); 

            var vm = new DebugDatabaseMenuViewModel();
            vm.Available = this.menuVersionDataService.IsAvailable(site.AndromediaSiteId);
            vm.LastError = this.menuVersionDataService.GetLastError();

            if (vm.Available)
            {
                var localData = this.menuVersionDataService.GetMenuVersionRow(site.AndromediaSiteId);
                var tempData = this.menuVersionDataService.GetTempMenuVersionRow(site.AndromediaSiteId);

                vm.MenuVersion = localData.nVersion;
                vm.MenuVersionLastUpdated = localData.tLastUpdated;

                vm.TempMenuVersion = tempData.nVersion;
                vm.MenuVersionLastUpdated = tempData.tLastUpdated;

                vm.AndromedaSiteId = this.site.AndromediaSiteId;
                vm.DbSiteId = localData.nSiteID.ToString();
                vm.DbMasterSiteId = localData.nMasterSiteID.ToString();
                vm.Message = "success";
            }
            else
            {
                vm.Message = "error";
                vm.ConnectionString = this.menuVersionDataService.GetConnectionString(site.AndromediaSiteId);
            }

            this.logger.Debug(string.Format("{0} : getStoreMenuVersion (siteid: {1}) completed", this.user.User.Username, this.site.SiteId)); 

            return vm;
        }

        public override Task OnConnected()
        {
            //must do this bit first
            
            //await this.JoinStoreContext();
            this.MyAndromedaJoin(this.user, this.site);

            //join group
            this.Clients.Caller.ping(string.Format("Store Hub connected ping: {0}", DateTime.UtcNow));
            this.Clients.Caller.user(this.user.User);

            return base.OnConnected(); 
        }

        public override Task OnReconnected()
        {
            //must do this bit first
            this.MyAndromedaReJoin(this.user, this.site);
            //await this.JoinStoreContext();

            this.Clients.Caller.ping(string.Format("Store Hub Reconnected ping: {0}", DateTime.UtcNow));

            return base.OnReconnected();
        }

    }
}