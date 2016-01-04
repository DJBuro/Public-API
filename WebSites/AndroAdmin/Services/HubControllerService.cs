using System.Linq;
using System.Web.SessionState;
using AndroAdmin.DataAccess;
using System;
using System.Collections.Generic;
using System.Web;
using AndroAdminDataAccess.DataAccess;
using AndroAdminDataAccess.Domain;

namespace AndroAdmin.Services
{
    public class HubControllerService
    {
        private readonly IHubDataService hubDataAccess = AndroAdminDataAccessFactory.GetHubDataService(); // new MockedHubDataServiceDAO();
        private readonly IStoreHubDataService storeHubDataAccess = AndroAdminDataAccessFactory.GetStoreHubDataService();

        public HubItem GetHub(Guid id)
        {
            var model = hubDataAccess.GetHub(id);

            return model;
        }

        public void UpdateHub(HubItem viewModel)
        {
            var dbModel = hubDataAccess.GetHub(viewModel.Id); 
            
            dbModel.Name = viewModel.Name;
            dbModel.Active = viewModel.Active;
            dbModel.Address = viewModel.Address;

            hubDataAccess.Update(dbModel);
        }

        /// <summary>
        /// Adds the hub.
        /// </summary>
        /// <param name="viewModel">The view model.</param>
        public void AddHub(HubItem viewModel)
        {
            hubDataAccess.Add(viewModel);
        }

        /// <summary>
        /// Removes the hub.
        /// </summary>
        /// <param name="hubId">The hub id.</param>
        public void RemoveHub(Guid hubId) 
        {
            var hub = this.hubDataAccess.GetHub(hubId);
            this.hubDataAccess.Remove(hub);
        }

        public IEnumerable<HubItem> GetHubs() 
        {
            return this.hubDataAccess.GetHubs();
        }

        public IEnumerable<StoreHub> GetSitesUsingHub(Guid id) 
        {
            return this.storeHubDataAccess.GetSitesUsingHub(id);   
        }
    }

    public class MockedHubDataServiceDAO : IHubDataService
    { 
        public IEnumerable<HubItem> GetAfterDataVersion(int fromVersion)
        {
            return this.Hubs;
        }

        public void Remove(HubItem dbModel)
        {
            this.Hubs.Remove(dbModel);
        }

        public void Add(HubItem dbModel)
        {
            this.Hubs.Add(dbModel);
        }

        public void Update(HubItem dbModel)
        {
            //nothing needed here!
        }

        public HubItem GetHub(Guid id)
        {
            var hubs = this.GetHubs();
            var hub = hubs.FirstOrDefault(e => e.Id == id);

            return hub;
        }


        public IEnumerable<HubItem> GetHubs()
        {
            return this.Hubs;
        }

        private System.Web.SessionState.HttpSessionState Session
        {
            get
            {
                return System.Web.HttpContext.Current.Session;
            }
        }

        private IEnumerable<HubItem> TestDump()
        {
            return new List<HubItem> { 
                new HubItem() {
                    Id = Guid.NewGuid(),
                    Name = "First",
                    Active = false,
                },
                new HubItem(){
                    Id = Guid.NewGuid(),
                    Name = "Second",
                    Active = true
                }
            };
        }

        private IEnumerable<StoreHub> TestDumpSiteHubs()
        {
            return new List<StoreHub> 
            { 
            };
        }


        private ICollection<HubItem> Hubs
        {
            get
            {
                if (this.Session["HubControllerService.GetAllHubs"] == null)
                    this.Session["HubControllerService.GetAllHubs"] = TestDump();

                return this.Session["HubControllerService.GetAllHubs"] as IList<HubItem>;
            }
        }

        private ICollection<StoreHub> SiteHubs
        {
            get
            {
                if (this.Session["HubControllerService.GetSitesUsingHub"] == null)
                    this.Session["HubControllerService.GetAllHubs"] = TestDumpSiteHubs();

                return this.Session["HubControllerService.GetAllHubs"] as IList<StoreHub>;
            }
        }
    }

    public class SiteHubControllerService 
    {
        private IStoreDAO storeDAO;
        private readonly IHubDataService hubDataService = AndroAdminDataAccessFactory.GetHubDataService(); //new MockedHubDataServiceDAO();
        private readonly IStoreHubDataService storeHubDataService = AndroAdminDataAccessFactory.GetStoreHubDataService(); //new MockedStoreHubDataService();
        private readonly IHubResetDataService hubResetDataService = AndroAdminDataAccessFactory.GetHubResetDataService();

        public void ResetSiteHubHardwareKey(int storeId)
        {
            hubResetDataService.ResetStore(storeId);
        }

        /// <summary>
        /// Store DAO
        /// </summary>
        public virtual IStoreDAO StoreDAO
        {
            get
            {
                if (this.storeDAO == null)
                {
                    this.storeDAO = AndroAdminDataAccessFactory.GetStoreDAO();
                }

                return this.storeDAO;
            }
            set { this.storeDAO = value; }
        }

        public void AddHubTo(Store store, Guid hubId)
        {
            var hub = hubDataService.GetHub(hubId);

            storeHubDataService.AddTo(store, hub);
        }

        public void RemoveHubFrom(Store store, StoreHub removeItem)
        {
            var hub = hubDataService.GetHub(removeItem.HubAddressId);
            storeHubDataService.RemoveFrom(store, hub);
        }
        
        public Store GetStore(int id) 
        {
            var store = this.StoreDAO.GetById(id);

            return store;
        }

        /// <summary>
        /// Gets the selected hubs for the store.
        /// </summary>
        /// <param name="storeId">The store id.</param>
        /// <returns></returns>
        public ICollection<StoreHub> GetSelectedHubs(int storeId)
        {
            var store = this.StoreDAO.GetById(storeId);
            var selected = storeHubDataService.GetSelectedHubs(store).ToList();

            return selected;   
        }
    }

    public class MockedStoreHubDataService : IStoreHubDataService
    {
        public IEnumerable<HubItem> GetAfterDataVersion(int fromVersion)
        {
            return this.HubItems;
        }

        public void RemoveFrom(Store store, HubItem removeItem)
        {
            var removeStoreHubs = this.StoreHubItems.Where(e => e.StoreExternalId == store.ExternalSiteId).ToArray();
            
            foreach (var item in removeStoreHubs) 
            {
                this.StoreHubItems.Remove(item);
            }
        }

        public void AddTo(Store store, HubItem hub)
        {
            var storeHub = new StoreHub() { 
                HubAddressId =  hub.Id,
                StoreExternalId = store.ExternalSiteId
            };

            this.StoreHubItems.Add(storeHub);
        }

        public IEnumerable<HubItem> GetHubItems()
        {
            return this.HubItems;
        }

        public IEnumerable<StoreHub> GetSelectedHubs(Store store)
        {
            return this.StoreHubItems.Where(e => e.StoreExternalId == store.ExternalSiteId);
        }


        public IEnumerable<StoreHub> GetSitesUsingHub(Guid id)
        {
            // TODO: Implement this method
            throw new NotImplementedException();
        }

        public HttpSessionState Session 
        {
            get 
            {
                return HttpContext.Current.Session;
            }
        }

        public ICollection<HubItem> HubItems
        {
            get 
            {
                if (Session["SiteHubControllerServiceSession.HubItems"] == null) 
                {
                    var hubs = new HubControllerService();

                    Session["SiteHubControllerServiceSession.HubItems"] = hubs.GetHubs().ToList();
                }

                var data= Session["SiteHubControllerServiceSession.HubItems"];
                return data as ICollection<HubItem>;
            }
        }

        public ICollection<StoreHub> StoreHubItems
        {
            get
            {
                if (Session["SiteHubControllerServiceSession.StoreHubItems"] == null)
                {
                    var hubs = new HubControllerService();

                    Session["SiteHubControllerServiceSession.StoreHubItems"] = new List<StoreHub>()
                    {
                    };
                }

                var data = Session["SiteHubControllerServiceSession.StoreHubItems"];
                return data as ICollection<StoreHub>;
            }
        }
    }
}