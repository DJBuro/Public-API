using System;
using System.Linq;
using MyAndromeda.Data.AcsServices.Context;
using MyAndromeda.Data.DataAccess.Menu;
using MyAndromeda.Data.Model.MyAndromeda;
using MyAndromeda.Framework.Contexts;
using MyAndromeda.Storage;

namespace MyAndromeda.Services.Media.Models
{
    public class ActiveMenuContext : IActiveMenuContext
    {
        private readonly IStorageService storageService;

        private readonly IMyAndromedaSiteMediaServerDataService myAndromedaSiteMediaServerService;
        private readonly IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService;

        private SiteMenu menu;
        private SiteMenuMediaServer mediaServer;
        
        public ActiveMenuContext(WorkContextWrapper workContextWrapper,
            IStorageService storage,
            IMyAndromedaSiteMediaServerDataService myAndromedaSiteMediaServerService,
            IMyAndromedaSiteMenuDataService myAndromedaSiteMenuDataService)
        {
            this.myAndromedaSiteMenuDataService = myAndromedaSiteMenuDataService;
            this.myAndromedaSiteMediaServerService = myAndromedaSiteMediaServerService;
            this.storageService = storage;

            if (workContextWrapper.Available) 
            {
                if (workContextWrapper.Current.CurrentSite.Available) 
                {
                    this.AndromedaSiteId = workContextWrapper.Current.CurrentSite.AndromediaSiteId;
                    this.ExternalSiteId = workContextWrapper.Current.CurrentSite.ExternalSiteId;
                }
            }
        }

        //public WorkContextWrapper WorkContextWrapper { get; private set; }

        public int AndromedaSiteId
        {
            get;
            private set;
        }

        public string ExternalSiteId
        {
            get;
            private set;
        }

        public SiteMenu Menu
        {
            get
            {
                if (this.menu != null)
                    return this.menu;

                if (AndromedaSiteId == 0)
                    return this.menu;

                return this.menu ?? (this.menu = this.myAndromedaSiteMenuDataService.GetMenu(AndromedaSiteId));
            }
            set 
            {
                this.menu = value;
            }
        }

        public SiteMenuMediaServer MediaServer
        { 
            get 
            {
                if (this.mediaServer != null)
                    return this.mediaServer;

                if (this.AndromedaSiteId == 0)
                    return null;

                return this.mediaServer ?? (this.mediaServer = this.myAndromedaSiteMediaServerService.GetMediaServerWithDefault(AndromedaSiteId));
            }
            set 
            {
                this.mediaServer = value;
            }
        }

        private string contentPath;
        
        /// <summary>
        /// Gets the content path.
        /// Takes the format of the content path in the database, replacing {0} with the host and {1} as the external id
        /// </summary>
        /// <value>The content path.</value>
        public string ContentPath
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(this.contentPath))
                {
                    return this.contentPath;
                }
                
                contentPath = 
                    this.storageService.ContentPath(this.MediaServer.Address, this.MediaServer.ContentPath, this.ExternalSiteId.ToLower());

                return contentPath;
            }
        }

        public void Setup(int andromedaSiteId, string externalSiteId)
        {
            this.AndromedaSiteId = andromedaSiteId;
            this.ExternalSiteId = externalSiteId;
        }
    }
}