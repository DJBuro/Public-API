using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Routing;
using MyAndromeda.Core.Site;
using MyAndromedaDataAccess;
using MyAndromedaDataAccess.Domain;
using MyAndromedaDataAccessEntityFramework;
using MyAndromedaDataAccessEntityFramework.DataAccess.Sites;
using MyAndromeda.Core.Services;
using MyAndromedaDataAccessEntityFramework.DataAccess.WebOrdering;
using MyAndromedaDataAccessEntityFramework.Model.AndroAdmin;
using MyAndromedaDataAccessEntityFramework.DataAccess.Users;

namespace MyAndromeda.Framework.Contexts
{
    //[DebuggerStepThrough]
    public class CurrentSite : ICurrentSite 
    {
        private readonly ICurrentRequest currentRequest;

        private readonly IUserAccessDataService userAccessDataService;
        private readonly IStoreDataService storeDataService;
        private readonly ISiteDataService siteDataService;
        private readonly IDataAccessFactory dataAccessFactory;
        private readonly IEnrolmentService enrolmentService;
        private readonly ICurrentUser currentUser;
        private readonly IWebOrderingWebSiteDataService webOrderingWebSiteDataService; 

        public CurrentSite(ICurrentRequest currentRequest,
            IDataAccessFactory dataAccessFactory,
            IUserAccessDataService userAccessDataService,
            ICurrentUser currentUser,
            ISiteDataService siteDataService,
            IEnrolmentService enrolmentService,
            IStoreDataService storeDataService,
            IWebOrderingWebSiteDataService webOrderingWebSiteDataService) 
        {
            this.webOrderingWebSiteDataService = webOrderingWebSiteDataService;
            this.storeDataService = storeDataService;
            this.currentRequest = currentRequest;
            this.enrolmentService = enrolmentService;
            this.siteDataService = siteDataService;
            this.currentUser = currentUser;
            this.userAccessDataService = userAccessDataService;
            this.dataAccessFactory = dataAccessFactory;
        }

        public bool Available
        {
            get
            {
                return this.Site != null;
            }
        }
        
        private IEnumerable<int> acsApplicationIds;
        public IEnumerable<int> AcsApplicationIds
        {
            get
            {
                if (acsApplicationIds != null)
                { return acsApplicationIds; }

                this.dataAccessFactory.SiteDataAccess.GetAcsApplicationIds(this.SiteId, out acsApplicationIds);

                return acsApplicationIds.ToArray();
            }
        }

        private IEnumerable<string> acsExternalApplicationIds;
        public IEnumerable<string> AcsExternalApplicationIds 
        {
            get 
            {
                if (acsExternalApplicationIds != null)
                    return acsExternalApplicationIds;

                this.dataAccessFactory.SiteDataAccess.GetExternalAcsApplicationIds(this.SiteId, out acsExternalApplicationIds);

                return acsExternalApplicationIds.ToArray();
            }
        }

        private IEnumerable<IEnrolmentLevel> _enrolmentLevels;
        public IEnumerable<IEnrolmentLevel> EnrolmentLevels
        {
            get
            {
                if(this.site == null)
                    return null;

                if (_enrolmentLevels != null)
                    return _enrolmentLevels;

                var siteLevels = this.enrolmentService.GetEnrolmentLevels(this.Site);
                _enrolmentLevels = siteLevels;

                return _enrolmentLevels;
            }
        }

        private bool? _authorizedAtSiteLevel;
        public bool AuthorizedAtSiteLevel
        {
            get
            {
                if (!_authorizedAtSiteLevel.HasValue) 
                {
                    if(!Available) 
                        return false;
                    _authorizedAtSiteLevel = this.userAccessDataService.IsTheUserAssociatedWithStore(this.currentUser.User.Id, this.SiteId);
                }

                return _authorizedAtSiteLevel.GetValueOrDefault();
            }
        }

        public int SiteId
        {
            get
            {
                return this.Available ? this.Site.Id : 0;
            }
        }

        public int AndromediaSiteId 
        {
            get
            {
                return this.Available ? this.Site.AndromediaSiteId : 0;
            }
        }

        public string ExternalSiteId
        {
            get
            {
                return this.Available ? this.Site.ExternalSiteId : string.Empty;
            }
        }

        public int ChainId
        {
            get
            {
                return this.Available ? this.Site.ChainId : 0;
            }
        }

        
        private Site site;
        public Site Site
        {
            get
            {
                bool load = this.site == null;
                if (load) { this.LoadSiteData(); }

                return site;
            }
        }

        private Store store;
        public Store Store 
        {
            get 
            {
                bool load = this.site == null;
                if (load) { this.LoadSiteData(); }

                return store;
            }
        }

        /// <summary>
        /// Loads the data.
        /// </summary>
        private void LoadSiteData()
        {
            //if (this.currentUser.User == null)
            //    return;

            //load by this route parameter 
            var siteExternalId = this.currentRequest.GetRouteData("ExternalSiteId");
            //or this one :) 
            var andromedaSiteIdParamerter = this.currentRequest.GetRouteData("AndromedaSiteId");

            if (
                (siteExternalId == null || string.IsNullOrWhiteSpace(siteExternalId.ToString())) && 
                (andromedaSiteIdParamerter == null || string.IsNullOrWhiteSpace(andromedaSiteIdParamerter.ToString()))
                ) 
            {
                IDictionary<string,object> allValues = this.currentRequest.RouteData.Values;
                
                var webApiParameterRoutes = allValues
                    .Where(e=> e.Value is IEnumerable<IHttpRouteData>)
                    .Select(e=> e.Value as IEnumerable<IHttpRouteData>)
                    .SelectMany(e=> e)
                    .SelectMany(e=> e.Values)
                    .ToArray();
                
                var externalSiteKeyValue = webApiParameterRoutes.FirstOrDefault(e => e.Key.Equals("ExternalSiteId", StringComparison.InvariantCultureIgnoreCase));
                var andromedaKeyValue = webApiParameterRoutes.FirstOrDefault(e=> e.Key.Equals("AndromedaSiteId", StringComparison.InvariantCultureIgnoreCase));

                if (!string.IsNullOrWhiteSpace(externalSiteKeyValue.ToString()))
                {
                    siteExternalId = externalSiteKeyValue.Value ?? "";
                }
                if (!string.IsNullOrWhiteSpace(andromedaKeyValue.ToString()))
                {
                    andromedaSiteIdParamerter = andromedaKeyValue.Value ?? "";
                }

            }

            
            string externalSiteId = siteExternalId.ToString();
            string andromedaSiteIdText = andromedaSiteIdParamerter.ToString();

            if (string.IsNullOrWhiteSpace(externalSiteId) && string.IsNullOrWhiteSpace(andromedaSiteIdText))
                return;

            Site site = null;
            Store store = null;

            if(!string.IsNullOrWhiteSpace(externalSiteId))
            {
                store = this.storeDataService.List(e => e.ExternalId == externalSiteId).SingleOrDefault();
                site = store.ToDomainModel();
            }

            if(site == null && !string.IsNullOrWhiteSpace(andromedaSiteIdText))
            {
                int androId = Convert.ToInt32(andromedaSiteIdText);
                store = this.storeDataService.List(e => e.AndromedaSiteId == androId).SingleOrDefault();
                site = siteDataService.List(e=> e.AndromedaSiteId == androId).SingleOrDefault();
            }
            
            if (site == null) return; 

            //bool accessible = this.userAccessDataService.IsTheUserAssociatedWithStore(this.currentUser.User.Id, site.Id);

            //if (!accessible)
            //    return;
            
            this.site = site;
            this.store = store;
        }

        public IEnumerable<MyAndromedaDataAccessEntityFramework.Model.AndroAdmin.AndroWebOrderingWebsite> AndroWebOrderingSites
        {
            get 
            {
                if (this.SiteId == 0)
                {
                    return Enumerable.Empty<MyAndromedaDataAccessEntityFramework.Model.AndroAdmin.AndroWebOrderingWebsite>();
                }

                return this.webOrderingWebSiteDataService.List(e => e.ACSApplication.ACSApplicationSites.Any(acsSite => acsSite.SiteId == this.SiteId)).ToArray();
            }
        }
    }
}